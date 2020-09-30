using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Reductech.Utilities.Testing
{
    public static class TestExtensions
    {
        /// <summary>
        /// Asserts that this task should result in success.
        /// </summary>
        public static async Task ShouldSucceed(this Task<Result> resultTask)
        {
            var r = await resultTask;
            r.ShouldBeSuccessful();
        }

        /// <summary>
        /// Asserts that this task should result in success.
        /// </summary>
        public static async Task ShouldSucceed<T>(this Task<Result<T>> resultTask)
        {
            var r = await resultTask;
            r.ShouldBeSuccessful();
        }

        /// <summary>
        /// Asserts that this result should equal the expected result.
        /// </summary>
        public static void ShouldBe<T>(this Result<T> result, Result<T> expectedResult)
        {
            if (expectedResult.IsSuccess)
            {
                result.ShouldBeSuccessful();
                result.Value.Should().Be(expectedResult.Value);
            }
            else
                result.ShouldBeFailure(expectedResult.Error);
        }

        /// <summary>
        /// Asserts that this result was successful.
        /// </summary>
        public static void ShouldBeSuccessful(this Result result)
        {
            var (_, isFailure, error) = result;
            Assert.False(isFailure, error);
        }

        /// <summary>
        /// Asserts that this result was successful.
        /// </summary>
        public static void ShouldBeSuccessful<T, TE>(this Result<T, TE> result, Func<TE, string> convert)
        {
            var (_, isFailure, _, error) = result;
            if (isFailure)
                throw new XunitException(convert(error));
        }

        /// <summary>
        /// Asserts that this result was successful.
        /// </summary>
        public static void ShouldBeSuccessful<T>(this Result<T> result)
        {
            var (_, isFailure, _, error) = result;

            if (isFailure)
                throw new XunitException(error);
        }


        /// <summary>
        /// Asserts that this result was a failure.
        /// </summary>
        public static void ShouldBeFailure(this Result result, string? expectedError = null)
        {
            result.IsFailure.Should().BeTrue();

            if (expectedError != null)
                result.Error.Should().Be(expectedError);
        }

        /// <summary>
        /// Asserts that this result was a failure.
        /// </summary>
        public static void ShouldBeFailure<T>(this Result<T> result, string? expectedError = null)
        {
            result.IsFailure.Should().BeTrue();

            if (expectedError != null)
                result.Error.Should().Be(expectedError);
        }

        /// <summary>
        /// Asserts that this result was a failure.
        /// </summary>
        public static void ShouldBeFailure<T, TE>(this Result<T, TE> result) => result.IsFailure.Should().BeTrue();

        /// <summary>
        /// Asserts that this result was a failure.
        /// </summary>
        public static void ShouldBeFailure<T, TE>(this Result<T, TE> result, TE expectedError)
        {
            result.IsFailure.Should().BeTrue();

            result.Error.Should().Be(expectedError);
        }


    }
}
