using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Converter.Proto.Analyze;
using gRpcurlUI.Core.Converter.Proto.Format;
using gRpcurlUI.Core.Model;
using gRpcurlUI.View.Dialog;
using gRpcurlUI.ViewModel.Dialog;
using gRpcurlUI.ViewModel.Dialog.Proto;
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

        public bool IsEnableExpansionCommand => Commands.Any();

        [ObservableProperty]
        private string version;

        [ObservableProperty]
        private string projectType;

        private readonly ObservableCollection<GrpcurlProject> projects = new();
        public IEnumerable<IProject> Projects => projects;

        public IEnumerable<ProjectExpansionCommand> Commands { get; }

        public GrpcurlProjectContext()
        {
            version = "1.0.0";
            projectType = "gRpcurl";
            WeakReferenceMessenger.Default.RegisterAll(this);

            Commands = new List<ProjectExpansionCommand>()
            {
                new ProjectExpansionCommand("Read Proto(Beta)", ReadProtoCommand)
            };
        }

        [RelayCommand]
        public void ReadProto()
        {
            var setting = new ProtoImportPageShareSetting();
            WizardDialog.ShowWizard("Read Proto(Beta)", new IWizardDialogViewModel[]
            {
                new ProtoImportPage1ViewModel(setting, DI.Get<IWindowService>()),
                new ProtoImportPage2ViewModel(setting, DI.Get<ProtoAnalyzeEntry>()),
                new ProtoImportPage3ViewModel(setting, DI.Get<ProtoFormatEntry>()),
            });
        }

        public bool RemoveProject(IProject project)
        {
            return projects.Remove((GrpcurlProject)project);
        }

        public void NewProject()
        {
            var project = new GrpcurlProject()
            {
                ProjectName = "new GrpcProject"
            };
            AddProject(project);
        }

        public void AddProject(IProject project)
        {
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
                else if (other.Projects == null || !other.Projects.Any())
                {
                    throw new Exception($"Export Error. Project is Nothing.");
                }

                foreach (var p in grpcurl.projects)
                {
                    AddProject(p);
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
