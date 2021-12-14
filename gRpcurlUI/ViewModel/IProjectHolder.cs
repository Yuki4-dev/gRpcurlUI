using gRpcurlUI.Model;

namespace gRpcurlUI.ViewModel
{
    public interface IProjectHolder<out T> where T : IProject
    {
        IProject SelectedProject { get; set; }

        IProjectContext<T> Context { get; }

        IProject Create(string projectName = null);

        void Add(IProject project);

        bool Remove(IProject project);
    }
}
