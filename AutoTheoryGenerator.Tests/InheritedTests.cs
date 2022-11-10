using System.Collections.Generic;
using Sequence.Utilities.AutoTheoryGenerator.TestsBase;

namespace Sequence.Utilities.AutoTheoryGenerator.Tests;

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
