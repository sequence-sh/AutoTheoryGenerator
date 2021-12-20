using System.Collections.Generic;
using Reductech.Utilities.AutoTheoryGenerator.TestsBase;

namespace Reductech.Utilities.AutoTheoryGenerator.Tests;

public partial  class ShapeTests : ColourTests
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
