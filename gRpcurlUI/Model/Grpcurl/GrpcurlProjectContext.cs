using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.Model.Grpcurl
{

    public class GrpcurlProjectContext : Observable, IProjectContext<GrpcurlProject>
    {
        public string Verion { get; set; }

        public string ProjectType { get; set; }

        private readonly ICollection<GrpcurlProject> projectsInternal = new ObservableCollection<GrpcurlProject>();
        public IEnumerable<GrpcurlProject> Projects => projectsInternal;

        public void AddPrject(GrpcurlProject project)
        {
            projectsInternal.Add(project);
        }

        public bool RemovePrject(GrpcurlProject project)
        {
            return projectsInternal.Remove(project);
        }

        public GrpcurlProjectContext()
        {
            Verion = "1.0.0";
            ProjectType = "gRpcurl";
        }
    }
}
