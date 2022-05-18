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

        public string Previous()
        {
            if (_currentIndex < 0 || _currentIndex >= _buffer.Count)
                return string.Empty;

            string result = _buffer[_currentIndex];
            _currentIndex--;
            return result;
        }

        public string Next()
        {
            int nextIndex = _currentIndex + 1;
            if (nextIndex < 0 || nextIndex >= _buffer.Count)
                return string.Empty;

            string result = _buffer[nextIndex];
            _currentIndex = nextIndex;
            return result;
        }
    }
}
