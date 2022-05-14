using System.Collections.Generic;

namespace gRpcurlUI.Model.Grpcurl
{
    public class AddGrpcProjectMessage
    {
        public IEnumerable<GrpcurlProject> Projects { get; }

        public AddGrpcProjectMessage(IEnumerable<GrpcurlProject> projects)
        {
            Projects = projects;
        }
    }
}
