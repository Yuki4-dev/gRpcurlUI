using System.Collections.Generic;
using System.ComponentModel;

namespace gRpcurlUI.Model
{
    public interface IProjectContext : INotifyPropertyChanged, INotifyPropertyChanging
    {
        string ProjectType { get; }

        string Version { get; }

        IEnumerable<IProject> Projects { get; }

        void Marge(IProjectContext other);

        void AddProject(IProject? project = null);

        bool RemoveProject(IProject project);

        object ToJsonObject();
    }
}
