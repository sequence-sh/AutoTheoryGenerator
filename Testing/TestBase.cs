using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace Reductech.Utilities.Testing
{
    /// <summary>
    /// A test with multiple cases
    /// </summary>
    public abstract class TestBase : IEnumerable<object[]>
    {
        /// <summary>
        /// Override this method and add [TheoryData] and [ClassData] attributes to it
        /// </summary>
        /// <param name="key"></param>
        public virtual void Test(string key)
        {
            var @case = _testCaseDictionary.Value[key];
            @case.Execute(TestOutputHelper);
        }


#pragma warning disable 8618 //The constructor must be parameterless. TestOutputHelper should be set directly from the public constructor.
        protected TestBase() => _testCaseDictionary = new Lazy<IReadOnlyDictionary<string, ITestBaseCase>>(() => MakeDictionary(TestCases));
#pragma warning restore 8618

        public ITestOutputHelper TestOutputHelper { get; set; }

        private readonly Lazy<IReadOnlyDictionary<string, ITestBaseCase>> _testCaseDictionary;


        protected abstract IEnumerable<ITestBaseCase> TestCases { get; }

        /// <inheritdoc />
        public IEnumerator<object[]> GetEnumerator() => TestCases.Select(testCase => new object[] { testCase.Name }).GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private static IReadOnlyDictionary<string, ITestBaseCase> MakeDictionary(IEnumerable<ITestBaseCase> testCases)
        {
            var groups = testCases.GroupBy(x => x.Name).ToList();

            var duplicateKeys = groups.Where(x => x.Count() > 1).Select(x => x.Key).Select(x => $"'{x}'").ToList();

            if (duplicateKeys.Any())
                throw new Exception($"Duplicate test names: {string.Join(", ", duplicateKeys)}");

            return groups.ToDictionary(x => x.Key, x => x.Single());
        }
    }
}
