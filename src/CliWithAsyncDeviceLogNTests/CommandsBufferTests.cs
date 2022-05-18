using CliWithAsyncDeviceLogN;
using FluentAssertions;
using Xunit;

namespace CliWithAsyncDeviceLogNTests
{
    public class CommandsBufferTests
    {
        [Fact]
        public void OneItemCommandHistoryTests()
        {
            CommandsBuffer buffer = new(maxSize: 1);
            buffer.Add("dir");
            buffer.Previous().Should().Be("dir");
        }
    }
}
