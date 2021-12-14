using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.Model.Grpcurl
{

    public class GrpcurlProjectContext : Observable, IProjectContext<GrpcurlProject>
    {
        private string _Verion;
        public string Verion
        {
            get => _Verion;
            set => OnPropertyChanged(ref _Verion, value);
        }

        private string _ProjectType;
        public string ProjectType
        {
            get => _ProjectType;
            set => OnPropertyChanged(ref _ProjectType, value);
        }

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
