using System.Collections.Generic;
using Reductech.Utilities.Testing.TestsBase;

namespace Reductech.Utilities.Testing.Tests
{

public partial class BirdTests : ColourTests
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

public partial class ShapeTests : ColourTests
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

}
