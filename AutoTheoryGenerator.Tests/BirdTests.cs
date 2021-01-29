using System.Collections.Generic;
using Reductech.Utilities.AutoTheoryGenerator.TestsBase;

namespace Reductech.Utilities.AutoTheoryGenerator.Tests
{

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

}
