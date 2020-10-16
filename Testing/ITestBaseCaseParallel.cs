using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Reductech.Utilities.Testing
{
    /// <summary>
    /// A single case of a TestBaseParallel test
    /// </summary>
    public interface ITestBaseCaseParallel
    {
        /// <summary>
        /// Unique name for the case.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Execute the test case.
        /// </summary>
        Task ExecuteAsync(ITestOutputHelper testOutputHelper);
    }
}