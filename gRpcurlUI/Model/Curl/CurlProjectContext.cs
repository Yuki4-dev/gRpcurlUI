using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gRpcurlUI.Model.Curl
{
    public class CurlProjectContext : Observable, IProjectContext
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
        public IEnumerable<IProject> Projects => projectsInternal;

        public CurlProjectContext()
        {
            Verion = "1.0.0";
            ProjectType = "curl";
        }

        public bool RemoveProject(IProject project)
        {
            return projectsInternal.Remove((CurlProject)project);
        }

        public void AddProject(IProject project = null)
        {
            if (project == null)
            {
                project = new CurlProject()
                {
                    ProjectName = "new CurlProject"
                };
            }
            projectsInternal.Add((CurlProject)project);
        }

        public void Marge(IProjectContext other)
        {
            if (other is CurlProjectContext curl)
            {
                if (Verion != other.Verion)
                {
                    throw new Exception($"Version Error. Export Version:{other.Verion} This Version:{Verion}");
                }
                else if (ProjectType != other.ProjectType)
                {
                    throw new Exception($"ProjectType Error. Export ProjectType:{other.ProjectType} This ProjectType:{ProjectType}");
                }
                else if (other.Projects == null || other.Projects.Count() == 0)
                {
                    throw new Exception($"Export Error. Project is Nothing");
                }

                foreach (var p in curl.projectsInternal)
                {
                    projectsInternal.Add(p);
                }
            }
            else
            {
                throw new Exception("Export Project is not CurlProjectContext");
            }
        }
    }
}
