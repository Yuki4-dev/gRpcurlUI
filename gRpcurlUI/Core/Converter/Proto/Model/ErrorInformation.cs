namespace gRpcurlUI.Core.Converter.Proto.Model
{
    public class ErrorInformation
    {
        public string TaskName { get; }

        public string Message { get; }

        public int Line { get; }

        public ErrorInformation(string taskName, string message, int line)
        {
            TaskName = taskName;
            Message = message;
            Line = line;
        }
    }
}
