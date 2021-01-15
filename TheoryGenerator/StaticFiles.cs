using System.Collections.Generic;
using static Reductech.Utilities.TheoryGenerator.Constants;

namespace Reductech.Utilities.TheoryGenerator
{

internal static class Constants
{
    public static readonly string GenerateTheoryAttribute = nameof(GenerateTheoryAttribute);
    public const string UseTestOutputHelperAttribute = nameof(UseTestOutputHelperAttribute);

    public static readonly string GenerateAsyncTheoryAttribute =
        nameof(GenerateAsyncTheoryAttribute);

    public static readonly string AutoTheory = nameof(AutoTheory);
    public const string ITestInstance = nameof(ITestInstance);
    public static readonly string IAsyncTestInstance = nameof(IAsyncTestInstance);
}

internal static class DefaultFiles
{
    public static readonly IEnumerable<(string fileName, string text)> StaticFiles =
        new List<(string fileName, string text)>()
        {
            (UseTestOutputHelperAttribute,
             $@"using System;

namespace {AutoTheory}
{{

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class UseTestOutputHelperAttribute : Attribute
    {{
    }}
}}"
            ),
            (GenerateTheoryAttribute, $@"using System;

namespace {AutoTheory}
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

namespace {AutoTheory}
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
            ("TestInstance", $@"using System.Threading.Tasks;
using Xunit.Abstractions;

namespace {AutoTheory}
{{
    public interface {ITestInstance} : IXunitSerializable
    {{
        public void Run(ITestOutputHelper testOutputHelper);
    }}

    public interface {IAsyncTestInstance} : IXunitSerializable
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

        /// <inheritdoc />
        public void Deserialize(IXunitSerializationInfo info)
        {{
            throw new System.NotImplementedException();
        }}

        /// <inheritdoc />
        public void Serialize(IXunitSerializationInfo info)
        {{
            info.AddValue(""Name"", ""Skip"");
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

        /// <inheritdoc />
        public void Deserialize(IXunitSerializationInfo info)
        {{
            throw new System.NotImplementedException();
        }}

        /// <inheritdoc />
        public void Serialize(IXunitSerializationInfo info)
        {{
            info.AddValue(""Name"", ""Skip"");
        }}
    }}
}}
")
        };
}

}
