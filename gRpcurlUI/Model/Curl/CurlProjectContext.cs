using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gRpcurlUI.Model.Curl
{
    public partial class CurlProjectContext : ObservableObject, IProjectContext
    {
        [ObservableProperty]
        private string version;

        [ObservableProperty]
        private string projectType;

        private readonly ICollection<CurlProject> projectsInternal = new ObservableCollection<CurlProject>();
        public IEnumerable<IProject> Projects => projectsInternal;

        public CurlProjectContext()
        {
            version = "1.0.0";
            projectType = "curl";
        }

        public bool RemoveProject(IProject project)
        {
            return projectsInternal.Remove((CurlProject)project);
        }

        public void AddProject(IProject? project = null)
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
                if (Version != other.Version)
                {
                    throw new Exception($"Version Error. Export Version:{other.Version} This Version:{Version}");
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
