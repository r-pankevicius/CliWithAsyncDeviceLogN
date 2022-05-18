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
            buffer.Next().Should().Be("dir");

            // Adding "cd" shall forget "dir"
            buffer.Add("cd");
            buffer.Previous().Should().Be("cd");
            buffer.Previous().Should().Be("");
            buffer.Next().Should().Be("cd");
            buffer.Next().Should().Be("");
        }

        [Fact]
        public void CommandBuffer_ArrowUpDownScenarios()
        {
            CommandsBuffer buffer = new(maxSize: 10);

            buffer.Add("1"); // 1 <ENTER>
            buffer.Add("2"); // 2 <ENTER>

            buffer.Next().Should().Be(""); // Arrow down
            buffer.Previous().Should().Be("2"); // Arrow up
            buffer.Previous().Should().Be("1"); // Arrow up
            buffer.Previous().Should().Be(""); // Arrow up
            buffer.Next().Should().Be("1");// Arrow down
            buffer.Next().Should().Be("2");// Arrow down
            buffer.Next().Should().Be(""); // Arrow down
        }
    }
}
