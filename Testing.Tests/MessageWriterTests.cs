using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Reductech.Utilities.Testing.Tests
{
    public class MessageWriterTests : MessageWriterTestCases
    {
        public MessageWriterTests(ITestOutputHelper testOutputHelper) => TestOutputHelper = testOutputHelper;

        /// <inheritdoc />
        [Theory]
        [ClassData(typeof(MessageWriterTestCases))]
        public override void Test(string key) => base.Test(key);
    }
    public class MessageWriterTestCases : TestBase
    {
        private static readonly string DefaultMessage = $"Hello there!{Environment.NewLine}";


        /// <inheritdoc />
        protected override IEnumerable<ITestBaseCase> TestCases
        {
            get
            {
                yield return new MessageWriterTestCase()
                {
                    CaseName = "When no args are supplied, writes default message to stream",
                    Args = Array.Empty<string>(),
                    Expected = DefaultMessage
                };

                yield return new MessageWriterTestCase()
                {
                    CaseName = "When first arg is an empty string, writes default message to stream",
                    Args = new[] {"", "abc"},
                    Expected = DefaultMessage
                };

                yield return new MessageWriterTestCase()
                {
                    CaseName = "When one arg is supplied, writes arg to stream",
                    Args = new[] {"Hiya!"},
                    Expected = $"Hiya!{Environment.NewLine}"
                };

                yield return new MessageWriterTestCase()
                {
                    CaseName = "When multiple arg are supplied, joins and writes all args to stream",
                    Args = new[] {"Hi", "there", "yourself"},
                    Expected = $"Hi there yourself{Environment.NewLine}"
                };
            }
        }


        private class MessageWriterTestCase : ITestBaseCase
        {
            /// <summary>
            /// The case name
            /// </summary>
            public string CaseName { get; set; } = null!;

            /// <summary>
            /// The expected message
            /// </summary>
            public string Expected { get; set; } = null!;

            /// <summary>
            /// Message arguments
            /// </summary>
            public string[] Args { get; set; } = null!;


            /// <inheritdoc />
            public string Name => CaseName;

            /// <inheritdoc />
            public void Execute(ITestOutputHelper testOutputHelper)
            {
                var messageStream = new MessageStream();
                var messageWriter = new MessageWriter(messageStream);

                messageWriter.WriteMessage(Args);
                var actual = messageStream.GetMessage();

                Assert.Equal(actual, Expected);
            }

            /// <inheritdoc />
            public override string ToString() => Name;


            private class MessageStream : IMessageStream
            {
                private readonly StringWriter _stringWriter = new StringWriter();

                public void WriteLine(string message) => _stringWriter.WriteLine(message);

                public string GetMessage() => _stringWriter.ToString();
            }
        }
    }
}
