using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Model;
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

        public bool IsEnableExpansionCommand => false;

        [ObservableProperty]
        private string version;

        [ObservableProperty]
        private string projectType;

        private readonly ICollection<CurlProject> projects = new ObservableCollection<CurlProject>();
        public IEnumerable<IProject> Projects => projects;

        public IEnumerable<ProjectExpansionCommand> Commands => new List<ProjectExpansionCommand>();


        public CurlProjectContext()
        {
            version = "1.0.0";
            projectType = "curl";
        }

        public bool RemoveProject(IProject project)
        {
            return projects.Remove((CurlProject)project);
        }

        public void NewProject()
        {
            var project = new CurlProject()
            {
                ProjectName = Language.Default.CurlProject.NewProject
            };
            AddProject(project);
        }

        public void AddProject(IProject project)
        {
            projects.Add((CurlProject)project);
        }

        public void Marge(IProjectContext other)
        {
            if (other is CurlProjectContext curl)
            {
                if (Version != other.Version)
                {
                    throw new Exception(string.Format(Language.Default.CurlProject.VersionError, other.Version, Version));
                }
                else if (ProjectType != other.ProjectType)
                {
                    throw new Exception(string.Format(Language.Default.CurlProject.ProjectTypeError, other.ProjectType, ProjectType));
                }
                else if (other.Projects == null || !other.Projects.Any())
                {
                    throw new Exception(Language.Default.CurlProject.ProjectNothing);
                }

                foreach (var p in curl.projects)
                {
                    AddProject(p);
                }
            }
            else
            {
                throw new Exception(Language.Default.CurlProject.ProjectTypeError);
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
                throw new Exception(Language.Default.CurlProject.NonJsonType);
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
