using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Reductech.Utilities.Testing
{

public abstract class TestBaseParallel : IEnumerable<object[]>
{
    /// <summary>
    /// Override this method and add [TheoryData] and [ClassData] attributes to it
    /// </summary>
    /// <param name="key"></param>
    public virtual async Task Test(string key)
    {
        var @case = _testCaseDictionary.Value[key];
        await @case.ExecuteAsync(TestOutputHelper);
    }

    #pragma warning disable 8618 //The constructor must be parameterless. TestOutputHelper should be set directly from the public constructor.
    protected TestBaseParallel() => _testCaseDictionary =
        new Lazy<IReadOnlyDictionary<string, ITestBaseCaseParallel>>(
            () => MakeDictionary(TestCases)
        );
    #pragma warning restore 8618

    public ITestOutputHelper TestOutputHelper { get; set; }

    private readonly Lazy<IReadOnlyDictionary<string, ITestBaseCaseParallel>> _testCaseDictionary;

    protected abstract IEnumerable<ITestBaseCaseParallel> TestCases { get; }

    /// <inheritdoc />
    public IEnumerator<object[]> GetEnumerator() =>
        TestCases.Select(testCase => new object[] { testCase.Name }).GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private static IReadOnlyDictionary<string, ITestBaseCaseParallel> MakeDictionary(
        IEnumerable<ITestBaseCaseParallel> testCases)
    {
        var groups = testCases.GroupBy(x => x.Name).ToList();

        var duplicateKeys = groups.Where(x => x.Count() > 1)
            .Select(x => x.Key)
            .Select(x => $"'{x}'")
            .ToList();

        if (duplicateKeys.Any())
            throw new Exception($"Duplicate test names: {string.Join(", ", duplicateKeys)}");

        return groups.ToDictionary(x => x.Key, x => x.Single());
    }
}

}
