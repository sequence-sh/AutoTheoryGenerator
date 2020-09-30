using Xunit.Abstractions;

namespace Reductech.Utilities.Testing
{
    /// <summary>
    /// A single case of a TestBase test
    /// </summary>
    public interface ITestBaseCase
    {
        /// <summary>
        /// Unique name for the case.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Execute the test case.
        /// </summary>
        /// <param name="testOutputHelper"></param>
        void Execute(ITestOutputHelper testOutputHelper);
    }
}
