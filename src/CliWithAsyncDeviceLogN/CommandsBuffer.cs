using System;
using System.Collections.Generic;

namespace CliWithAsyncDeviceLogN
{
    /// <summary>
    /// A.K.A. "Command history".
    /// </summary>
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

        public void Add(string command)
        {
            if (string.IsNullOrEmpty(command))
                return;

            if (_buffer.Count > 0 && _buffer[_buffer.Count - 1] == command)
                return;

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
            if (_currentIndex < 0 || _currentIndex + 1 >= _buffer.Count)
                return string.Empty;

            string result = _buffer[_currentIndex + 1];
            _currentIndex++;
            return result;
        }
    }
}
