using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gRpcurlUI.Model.Grpcurl
{
    public partial class GrpcurlProjectContext : ObservableObject, IProjectContext, IRecipient<AddGrpcProjectMessage>
    {
        [ObservableProperty]
        private string version;

        [ObservableProperty]
        private string projectType;

        private readonly ICollection<GrpcurlProject> projectsInternal = new ObservableCollection<GrpcurlProject>();
        public IEnumerable<IProject> Projects => projectsInternal;

        public GrpcurlProjectContext()
        {
            version = "1.0.0";
            projectType = "gRpcurl";
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        public bool RemoveProject(IProject project)
        {
            return projectsInternal.Remove((GrpcurlProject)project);
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
            projectsInternal.Add((GrpcurlProject)project);
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

                foreach (var p in grpcurl.projectsInternal)
                {
                    projectsInternal.Add(p);
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
    }
}
