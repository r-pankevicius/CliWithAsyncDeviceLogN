using CliWithAsyncDeviceLogN;
using FluentAssertions;
using Xunit;

namespace CliWithAsyncDeviceLogNTests
{
    public class CommandsBufferTests
    {
        [Fact]
        public void Next_AfterAdd_ShouldReturnEmptyString()
        {
            CommandsBuffer buffer = new(maxSize: 1);

            buffer.Next().Should().Be("");

            buffer.Add("dir");

            buffer.Next().Should().Be("");
        }

        [Fact]
        public void Previous_AfterAdd_ShouldReturnLastString()
        {
            CommandsBuffer buffer = new(maxSize: 2);

            buffer.Previous().Should().Be("");

            buffer.Add("dir");
            buffer.Previous().Should().Be("dir");

            buffer.Add("cd");
            buffer.Previous().Should().Be("cd");
            buffer.Previous().Should().Be("dir");
            buffer.Previous().Should().Be("");
        }

        [Fact]
        public void CommandBuffer_OfMaxSize1_ShouldKeepOnly1ItemInHistory()
        {
            CommandsBuffer buffer = new(maxSize: 1);

            buffer.Add("dir");
            buffer.Previous().Should().Be("dir");
            buffer.Next().Should().Be("");

            buffer.Add("cd");
            buffer.Previous().Should().Be("cd");
            buffer.Next().Should().Be("");
        }
    }
}
