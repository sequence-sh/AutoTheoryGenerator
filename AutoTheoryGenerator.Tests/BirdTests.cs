using System.Collections.Generic;
using Sequence.Utilities.AutoTheoryGenerator.TestsBase;

namespace Sequence.Utilities.AutoTheoryGenerator.Tests;

public partial class BirdTests : ColourTests
{
    partial void OnInitialized() { }

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
