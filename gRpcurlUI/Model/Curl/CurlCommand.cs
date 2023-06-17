using gRpcurlUI.Core.Process;

namespace gRpcurlUI.Model.Curl
{
    public class CurlCommand : IProcessCommand
    {
        public string AppName { get; } = "curl";

        public string Arguments { get; }

        public CurlCommand(string option, string endPoint, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                Arguments = $"{option} {endPoint}";
            }
            else
            {
                Arguments = $"{option} -d \"{Replace(content)}\" {endPoint}";
            }
        }

        private static string Replace(string text)
        {
            return text.Replace("\"", "\\\"").Replace("\r\n", " ").Trim();
        }
    }
}
