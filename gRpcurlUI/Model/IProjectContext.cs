using System.Collections.Generic;

namespace gRpcurlUI.Model
{
    public interface IProjectContext
    {
        string ProjectType { get; }

        string Verion { get; }

        IEnumerable<IProject> Projects { get; }

        void SetSetting(IReadOnlyAppSetting setting);

        void Validate(IProjectContext other);

        void AddProject(IProject project = null);

        bool RemoveProject(IProject project);
    }
}
