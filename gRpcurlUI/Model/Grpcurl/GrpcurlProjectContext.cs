using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace gRpcurlUI.Model.Grpcurl
{

    public class GrpcurlProjectContext : Observable, IProjectContext
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
        public IEnumerable<IProject> Projects => projectsInternal;

        private IReadOnlyAppSetting appSetting;

        public GrpcurlProjectContext()
        {
            Verion = "1.0.0";
            ProjectType = "gRpcurl";
        }

        public void SetSetting(IReadOnlyAppSetting setting)
        {
            appSetting = setting;
        }

        public bool RemoveProject(IProject project)
        {
            return projectsInternal.Remove((GrpcurlProject)project);
        }

        public void AddProject(IProject project = null)
        {
            if (project == null)
            {
                project = new GrpcurlProject()
                {
                    AppPath = appSetting?.AppPath,
                    ProjectName = "new Project"
                };
            }
            projectsInternal.Add((GrpcurlProject)project);
        }

        public void Validate(IProjectContext other)
        {
            if (other is GrpcurlProjectContext)
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
                    throw new Exception($"Export Error. Project is Nothing.");
                }
            }
            else
            {
                throw new Exception("Export Project is not GrpcurlProject");
            }
        }

    }
}
