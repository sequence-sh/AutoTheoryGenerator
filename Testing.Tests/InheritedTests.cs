using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Reductech.Utilities.Testing.Tests
{
    public partial class BirdTests : InheritedTests
    {
        /// <inheritdoc />
        public override IEnumerable<string> BasicCases
        {
            get
            {
                yield return "Parrot";
                yield return "Robin";
            }
        }
    }

    public partial class ShapeTests : InheritedTests
    {
        /// <inheritdoc />
        public override IEnumerable<string> BasicCases
        {
            get
            {
                yield return "Square";
                yield return "Circle";
            }
        }
    }

    public abstract class InheritedTests
    {

        public abstract IEnumerable<string> BasicCases { get; }

        [AutoTheory.GenerateTheory("Red")]
        public IEnumerable<TestInstance> RedCases => BasicCases.Select(x =>new TestInstance(x + "Red"));

        [AutoTheory.GenerateTheory("Blue")]
        public IEnumerable<TestInstance> BlueCases => BasicCases.Select(x =>new TestInstance(x + "Blue"));


        public record TestInstance(string String) : AutoTheory.ITestInstance//, IXunitSerializable
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
