using System;

namespace gRpcurlUI.Core.Reader
{
    public class ProtoReader : IObjectTextReader
    {
        private object? _buffer;

        public bool IsReading { get; private set; } = false;

        public bool IsComplete { get; private set; } = false;

        public object? GetObject(bool clear)
        {
            if (!IsComplete)
            {
                throw new InvalidOperationException("Not Complete Read.");
            }
            IsComplete = false;

            var obj = _buffer;
            if (clear)
            {
                _buffer = null;
            }

            return obj;
        }

        public bool ReadLine(string line, out string message)
        {
            var result = true;
            message = "";
            if (IsStartLine(line))
            {
                if (IsReading)
                {
                    result = false;
                    message = "overwrite";
                }
                CreateObject(line);
            }
            else if (IsEndLine(line))
            {
                Complete();
            }
            else
            {
                if (IsReading)
                {

                }
                else
                {
                    result = false;
                    message = "ignore line :" + line;
                }
            }

            return result;
        }

        private bool IsStartLine(string line)
        {
            return line.StartsWith("message");
        }

        private bool IsEndLine(string line)
        {
            return IsReading && line.StartsWith("}");
        }

        private bool CreateObject(string line)
        {
            _buffer = null;
            IsReading = true;
            IsComplete = false;

            return true;
        }

        private void Complete()
        {
            IsReading = false;
            IsComplete = true;
        }

    }
}
