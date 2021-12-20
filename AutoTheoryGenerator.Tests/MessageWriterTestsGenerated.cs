﻿using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Reductech.Utilities.AutoTheoryGenerator.Tests;

[AutoTheory.UseTestOutputHelper]
public partial class MessageWriterTests
{
    private static readonly string DefaultMessage = $"Hello there!{Environment.NewLine}";

    [AutoTheory.GenerateTheory("Test")]
    private IEnumerable<MessageWriterTestCase> TestCases
    {
        get
        {
            yield return new MessageWriterTestCase()
            {
                CaseName = "When no args are supplied, writes default message to stream",
                Args     = Array.Empty<string>(),
                Expected = DefaultMessage
            };

            yield return new MessageWriterTestCase()
            {
                CaseName =
                    "When first arg is an empty string, writes default message to stream",
                Args     = new[] { "", "abc" },
                Expected = DefaultMessage
            };

            yield return new MessageWriterTestCase()
            {
                CaseName = "When one arg is supplied, writes arg to stream",
                Args     = new[] { "Hiya!" },
                Expected = $"Hiya!{Environment.NewLine}"
            };

            yield return new MessageWriterTestCase()
            {
                CaseName =
                    "When multiple arg are supplied, joins and writes all args to stream",
                Args     = new[] { "Hi", "there", "yourself" },
                Expected = $"Hi there yourself{Environment.NewLine}"
            };
        }
    }

    private class MessageWriterTestCase : AutoTheory.ITestInstance, IXunitSerializable
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

        public string Name => CaseName;

        /// <inheritdoc />
        public void Run(ITestOutputHelper testOutputHelper)
        {
            var messageStream = new MessageStream();
            var messageWriter = new MessageWriter(messageStream);

            messageWriter.WriteMessage(Args);
            var actual = messageStream.GetMessage();

            Assert.Equal(actual, Expected);
        }

        /// <inheritdoc />
        public override string ToString() => Name;

        /// <inheritdoc />
        public void Deserialize(IXunitSerializationInfo info)
        {
            CaseName = info.GetValue<string>(nameof(CaseName));
            Expected = info.GetValue<string>(nameof(Expected));
            Args     = info.GetValue<string[]>(nameof(Args));
        }

        /// <inheritdoc />
        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(CaseName), CaseName);
            info.AddValue(nameof(Expected), Expected);
            info.AddValue(nameof(Args),     Args);
        }

        private class MessageStream : IMessageStream
        {
            private readonly StringWriter _stringWriter = new StringWriter();

            public void WriteLine(string message) => _stringWriter.WriteLine(message);

            public string GetMessage() => _stringWriter.ToString();
        }
    }
}
