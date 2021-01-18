using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Reductech.Utilities.Testing.TestsBase
{


    public abstract class ColourTests
    {
        public abstract IEnumerable<string> BasicCases { get; }

        [AutoTheory.GenerateTheory("Red")]
        public IEnumerable<TestInstance> RedCases =>
            BasicCases.Select(x => new TestInstance(x + "Red"));

        [AutoTheory.GenerateTheory("Blue")]
        public IEnumerable<TestInstance> BlueCases =>
            BasicCases.Select(x => new TestInstance(x + "Blue"));

        public class TestInstance : AutoTheory.ITestInstance
        {
            public TestInstance(string s) {
                String = s;
            }

            /// <inheritdoc />
            public void Run(ITestOutputHelper testOutputHelper)
            {
                testOutputHelper.WriteLine(String);
            }

            public string Name => String;

            public string String
            {
                get;
                set;
            }
        }
    }
}
