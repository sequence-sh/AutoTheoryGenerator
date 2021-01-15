using System.Collections.Generic;
using static Reductech.Utilities.TheoryGenerator.Constants;

namespace Reductech.Utilities.TheoryGenerator
{

    internal static class Constants
    {
        public static readonly string GenerateTheoryAttribute = nameof(GenerateTheoryAttribute);
        public static readonly string UseTestOutputHelperAttribute = nameof(UseTestOutputHelperAttribute);
        public static readonly  string GenerateAsyncTheoryAttribute = nameof(GenerateAsyncTheoryAttribute);
        public static readonly  string AutoTheory = nameof(AutoTheory);
        public static readonly  string ITestInstance = nameof(ITestInstance);
        public static readonly  string IAsyncTestInstance = nameof(IAsyncTestInstance);
    }
    internal static class DefaultFiles
    {
        public static readonly IEnumerable<(string fileName, string text)> StaticFiles =
            new List<(string fileName, string text)>()
            {
                (UseTestOutputHelperAttribute,

                    $@"using System;

namespace {Constants.AutoTheory}
{{

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class UseTestOutputHelperAttribute : Attribute
    {{
    }}
}}"
                    ),



                (GenerateTheoryAttribute, $@"using System;

namespace {Constants.AutoTheory}
{{

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class {GenerateTheoryAttribute} : Attribute
    {{
        public {GenerateTheoryAttribute}(string testName)
        {{
            TestName = testName;
        }}

        public string TestName {{ get; set; }}
        public string Category {{ get; set; }}
    }}
}}"),
                (GenerateAsyncTheoryAttribute, $@"using System;

namespace {Constants.AutoTheory}
{{

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class {GenerateAsyncTheoryAttribute} : Attribute
    {{
        public {GenerateAsyncTheoryAttribute}(string testName)
        {{
            TestName = testName;
        }}

        public string TestName {{ get; set; }}
        public string Category {{ get; set; }}
    }}
}}"),

                ("TestInstance",$@"using System.Threading.Tasks;
using Xunit.Abstractions;

namespace {Constants.AutoTheory}
{{
    public interface {ITestInstance}
    {{
        public void Run(ITestOutputHelper testOutputHelper);
    }}

    public interface {IAsyncTestInstance}
    {{
        public Task RunAsync(ITestOutputHelper testOutputHelper);
    }}

    public class SkipTestInstance : {ITestInstance}, {IAsyncTestInstance}
    {{
        public void Run(ITestOutputHelper testOutputHelper)
        {{
            testOutputHelper.WriteLine(""This test was skipped as there are no cases."");
        }}

        public async Task RunAsync(ITestOutputHelper testOutputHelper)
        {{
            testOutputHelper.WriteLine(""This test was skipped as there are no cases."");
            await Task.CompletedTask;
        }}
    }}

    public class NullTestInstance : {ITestInstance}, {IAsyncTestInstance}
    {{
        public void Run(ITestOutputHelper testOutputHelper)
        {{
            throw new Xunit.Sdk.XunitException(""The test case source was null"");
        }}

        public Task RunAsync(ITestOutputHelper testOutputHelper)
        {{
            throw new Xunit.Sdk.XunitException(""The test case source was null"");
        }}
    }}
}}
")
            };
    }
}
