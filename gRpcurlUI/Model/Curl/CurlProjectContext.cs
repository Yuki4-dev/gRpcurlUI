using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.Model.Curl
{
    public class CurlProjectContext : Observable, IProjectContext<CurlProject>
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

        private readonly ICollection<CurlProject> projectsInternal = new ObservableCollection<CurlProject>();
        public IEnumerable<CurlProject> Projects => projectsInternal;

        public void AddPrject(CurlProject project)
        {
            projectsInternal.Add(project);
        }

        public bool RemovePrject(CurlProject project)
        {
            return projectsInternal.Remove(project);
        }

        public CurlProjectContext()
        {
            Verion = "1.0.0";
            ProjectType = "curl";
        }
    }
}
