using gRpcurlUI.Core;

namespace gRpcurlUI.Model.Curl
{
    public class CurlCommand : IProccesCommand
    {
        public string AppPath { get; } = "curl";

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

        private string Replace(string text)
        {
            return text.Replace("\"", "\\\"").Replace("\r\n"," ").Trim();
        }
    }
}
