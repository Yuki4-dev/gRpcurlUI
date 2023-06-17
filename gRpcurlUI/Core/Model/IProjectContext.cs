using System.Collections.Generic;
using System.ComponentModel;

namespace gRpcurlUI.Core.Model
{
    public interface IProjectContext : IJsonObject, INotifyPropertyChanged, INotifyPropertyChanging
    {
        string ProjectType { get; }

        string Version { get; }

        bool IsEnableExpansionCommand { get; }

        IEnumerable<IProject> Projects { get; }

        IEnumerable<ProjectExpansionCommand> Commands { get; }

        void Marge(IProjectContext other);

        void NewProject();

        void AddProject(IProject project);

        bool RemoveProject(IProject project);
    }
}
