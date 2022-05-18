using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CliWithAsyncDeviceLogN
{
    /// <summary>
    /// A.K.A. "Command history".
    /// </summary>
    [DebuggerDisplay("CommandsBuffer. Count: {Count}, Index: {Index}")]
    internal class CommandsBuffer
    {
        private readonly List<string> _buffer;
        private readonly int _maxSize;
        private int _currentIndex;

        public CommandsBuffer(int maxSize)
        {
            _maxSize = maxSize > 0 ? maxSize : throw new ArgumentOutOfRangeException(nameof(maxSize));
            _buffer = new();
            _currentIndex = -1;
        }

        public int Count => _buffer.Count;
        public int Index => _currentIndex;

        /// <summary>
        /// Adds <paramref name="command"/> to the buffer.
        /// (command ENTER)
        /// </summary>
        public void Add(string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            if (_buffer.Count > 0 && _buffer[^1] == command)
                return;

            if (_buffer.Count == _maxSize)
                _buffer.RemoveAt(0);

            _buffer.Add(command);
            _currentIndex = _buffer.Count - 1;
        }

        /// <summary>
        /// Gets previous command from the buffer or empty string if there is nothing there.
        /// (UP ARROW)
        /// </summary>
        public string Previous()
        {
            int previousIndex = _currentIndex;
            if (previousIndex < 0 || previousIndex >= _buffer.Count)
                return string.Empty;

            string result = _buffer[previousIndex];
            _currentIndex = previousIndex - 1;
            return result;
        }

        /// <summary>
        /// Gets next command from the buffer or empty string if there is nothing there.
        /// (DOWN ARROW)
        /// </summary>
        public string Next()
        {
            int nextIndex = _currentIndex + 1;
            if (nextIndex == 0)
                nextIndex++;

            if (nextIndex < 0 || nextIndex >= _buffer.Count)
                return string.Empty;

            string result = _buffer[nextIndex];
            _currentIndex = nextIndex;
            return result;
        }
    }
}
