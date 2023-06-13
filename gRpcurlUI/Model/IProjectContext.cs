using gRpcurlUI.Core.Model;
using System.Collections.Generic;
using System.ComponentModel;

namespace gRpcurlUI.Model
{
    public interface IProjectContext : IJsonObject, INotifyPropertyChanged, INotifyPropertyChanging
    {
        string ProjectType { get; }

        string Version { get; }

        IEnumerable<IProject> Projects { get; }

        void Marge(IProjectContext other);

        void NewProject();

        void AddProject(IProject project);

        bool RemoveProject(IProject project);
    }
}
