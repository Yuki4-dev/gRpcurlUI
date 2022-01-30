using gRpcurlUI.Core.Procces;

namespace gRpcurlUI.Model.Curl
{
    public class CurlCommand : IProccesCommand
    {
        public string AppPath { get; }

        public string Arguments { get; }

        public CurlCommand(string appPath, string option, string endPoint, string content)
        {
            AppPath = appPath;

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
            return text.Replace("\"", "\\\"").Replace("\r\n", " ").Trim();
        }
    }
}
