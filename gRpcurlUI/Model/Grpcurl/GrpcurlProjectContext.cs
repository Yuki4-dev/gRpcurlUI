using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static gRpcurlUI.Model.Grpcurl.GrpcurlProject;

namespace gRpcurlUI.Model.Grpcurl
{
    public partial class GrpcurlProjectContext : ObservableObject, IProjectContext, IRecipient<AddGrpcProjectMessage>
    {
        public Type JsonType => typeof(GrpcProjectContextJson);

        [ObservableProperty]
        private string version;

        [ObservableProperty]
        private string projectType;

        private readonly ICollection<GrpcurlProject> projects = new ObservableCollection<GrpcurlProject>();
        public IEnumerable<IProject> Projects => projects;

        public GrpcurlProjectContext()
        {
            version = "1.0.0";
            projectType = "gRpcurl";
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public bool RemoveProject(IProject project)
        {
            return projects.Remove((GrpcurlProject)project);
        }

        public void AddProject(IProject? project = null)
        {
            if (project == null)
            {
                project = new GrpcurlProject()
                {
                    ProjectName = "new GrpcProject"
                };
            }
            projects.Add((GrpcurlProject)project);
        }

        public void Marge(IProjectContext other)
        {
            if (other is GrpcurlProjectContext grpcurl)
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
                    throw new Exception($"Export Error. Project is Nothing.");
                }

                foreach (var p in grpcurl.projects)
                {
                    projects.Add(p);
                }
            }
            else
            {
                throw new Exception("Export Project is not GrpcurlProject");
            }
        }

        public void Receive(AddGrpcProjectMessage message)
        {
            foreach (var p in message.Projects)
            {
                AddProject(p);
            }
        }

        public object ToJsonObject()
        {
            return new GrpcProjectContextJson()
            {
                ProjectType = ProjectType,
                Version = Version,
                Projects = projects.Select(p => (GrpcProjectJson)p.ToJsonObject())
            };
        }

        public void LoadJsonObject(object jsonObject)
        {
            if (jsonObject is not GrpcProjectContextJson grpcContext)
            {
                throw new Exception("Json is not GrpcProjectContext.");
            }

            ProjectType = grpcContext.ProjectType;
            Version = grpcContext.Version;

            var grpcProjects = grpcContext.Projects.Select(json =>
            {
                var grpcProject = new GrpcurlProject();
                grpcProject.LoadJsonObject(json);
                return grpcProject;
            });

            projects.Clear();
            foreach (var grpcProject in grpcProjects)
            {
                projects.Add(grpcProject);
            }
        }

        private class GrpcProjectContextJson
        {
            public string ProjectType { get; set; } = string.Empty;

            public string Version { get; set; } = string.Empty;

            public IEnumerable<GrpcProjectJson> Projects { get; set; } = new List<GrpcProjectJson>();
        }
    }
}
