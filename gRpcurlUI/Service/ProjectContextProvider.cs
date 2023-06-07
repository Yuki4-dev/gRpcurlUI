using gRpcurlUI.Core.API;
using gRpcurlUI.Model;
using gRpcurlUI.Model.Curl;
using gRpcurlUI.Model.Grpcurl;

namespace gRpcurlUI.Service
{
    public class ProjectContextProvider : IProjectContextProvider
    {
        private readonly IProjectContext grpcProjectContext = new GrpcurlProjectContext();

        private readonly IProjectContext curlProjectContext = new CurlProjectContext();

        public IProjectContext[] GetProjectContexts()
        {
            return new IProjectContext[]
            {
                grpcProjectContext,
                curlProjectContext,
            };
        }
    }
}
