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

        public record TestInstance(string String) : AutoTheory.ITestInstance //, IXunitSerializable
        {
            /// <inheritdoc />
            public void Run(ITestOutputHelper testOutputHelper)
            {
                testOutputHelper.WriteLine(String);
            }

            /// <inheritdoc />
            public void Deserialize(IXunitSerializationInfo info)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc />
            public void Serialize(IXunitSerializationInfo info)
            {
                info.AddValue("Name", "Skip");
            }
        }
    }
}
