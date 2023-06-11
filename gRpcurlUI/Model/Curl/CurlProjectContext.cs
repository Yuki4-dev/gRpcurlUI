using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static gRpcurlUI.Model.Curl.CurlProject;

namespace gRpcurlUI.Model.Curl
{
    public partial class CurlProjectContext : ObservableObject, IProjectContext
    {
        public Type JsonType => typeof(CurlProjectContextJson);

        [ObservableProperty]
        private string version;

        [ObservableProperty]
        private string projectType;

        private readonly ICollection<CurlProject> projects = new ObservableCollection<CurlProject>();
        public IEnumerable<IProject> Projects => projects;

        public CurlProjectContext()
        {
            version = "1.0.0";
            projectType = "curl";
        }

        public bool RemoveProject(IProject project)
        {
            return projects.Remove((CurlProject)project);
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
            projects.Add((CurlProject)project);
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

                foreach (var p in curl.projects)
                {
                    projects.Add(p);
                }
            }
            else
            {
                throw new Exception("Export Project is not CurlProjectContext");
            }
        }

        public object ToJsonObject()
        {
            return new CurlProjectContextJson()
            {
                ProjectType = ProjectType,
                Version = Version,
                Projects = projects.Select(p => (CurlProjectJson)p.ToJsonObject())
            };
        }

        public void LoadJsonObject(object jsonObject)
        {
            if (jsonObject is not CurlProjectContextJson curlContext)
            {
                throw new Exception("Json is not CurlProjectContext.");
            }

            ProjectType = curlContext.ProjectType;
            Version = curlContext.Version;

            var curlProjects = curlContext.Projects.Select(json =>
            {
                var curlProject = new CurlProject();
                curlProject.LoadJsonObject(json);
                return curlProject;
            });


            projects.Clear();
            foreach (var curlProject in curlProjects)
            {
                projects.Add(curlProject);
            }
        }


        private class CurlProjectContextJson
        {
            public string ProjectType { get; set; } = string.Empty;

            public string Version { get; set; } = string.Empty;

            public IEnumerable<CurlProjectJson> Projects { get; set; } = new List<CurlProjectJson>();
        }
    }
}
