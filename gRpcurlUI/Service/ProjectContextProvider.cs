using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Model;
using gRpcurlUI.Model.Curl;
using gRpcurlUI.Model.Grpcurl;

namespace gRpcurlUI.Service
{
    public class ProjectContextProvider : IProjectContextProvider
    {
        private readonly GrpcurlProjectContext grpcProjectContext;

        private readonly CurlProjectContext curlProjectContext;

        public ProjectContextProvider(GrpcurlProjectContext grpcProjectContext, CurlProjectContext curlProjectContext)
        {
            this.grpcProjectContext = grpcProjectContext;
            this.curlProjectContext = curlProjectContext;
        }

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
