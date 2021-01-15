using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit.Abstractions;

namespace Reductech.Utilities.Testing.Tests
{
    public partial class BirdTests : InheritedTests
    {
        /// <inheritdoc />
        public override IEnumerable<TestInstance> BasicCases
        {
            get
            {
                yield return new TestInstance("Parrot");
                yield return new TestInstance("Robin");
            }
        }
    }

    public partial class ShapeTests : InheritedTests
    {
        /// <inheritdoc />
        public override IEnumerable<TestInstance> BasicCases
        {
            get
            {
                yield return new TestInstance("Square");
                yield return new TestInstance("Circle");
            }
        }
    }

    public abstract class InheritedTests
    {

        public abstract IEnumerable<TestInstance> BasicCases { get; }

        [AutoTheory.GenerateTheory("Red")]
        public IEnumerable<TestInstance> RedCases => BasicCases.Select(x => x with {String = x.String + "Red"});

        [AutoTheory.GenerateTheory("Blue")]
        public IEnumerable<TestInstance> BlueCases => BasicCases.Select(x => x with {String = x.String + "Blue"});


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
