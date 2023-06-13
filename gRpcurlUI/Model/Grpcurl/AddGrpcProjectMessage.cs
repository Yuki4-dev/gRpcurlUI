namespace gRpcurlUI.Model.Grpcurl
{
    public class AddGrpcProjectMessage
    {
        public GrpcurlProject[] Projects { get; }

        public AddGrpcProjectMessage(GrpcurlProject[] projects)
        {
            Projects = projects;
        }
    }
}
