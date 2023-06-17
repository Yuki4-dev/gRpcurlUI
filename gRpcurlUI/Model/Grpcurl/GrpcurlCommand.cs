using gRpcurlUI.Core.Process;

namespace gRpcurlUI.Model.Grpcurl
{
    public class GrpcurlCommand : IProcessCommand
    {
        public string AppName { get; }

        public string Arguments { get; }

        public GrpcurlCommand(string appPath, string option, string endPoint, string content, string service)
        {
            AppName = appPath;
            Arguments = string.IsNullOrWhiteSpace(content)
                ? $"{option} {endPoint} {service}"
                : $"{option} -d \"{Replace(content)}\" {endPoint} {service}";
        }

        private static string Replace(string text)
        {
            return text.Replace("\"", "\\\"").Trim();
        }
    }
}
