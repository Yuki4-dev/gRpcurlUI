using gRpcurlUI.Model;

namespace gRpcurlUI.ViewModel
{
    public interface IProjectHolder<out T> where T : IProject
    {
        IProject SelectedProject { get; set; }

        IProjectContext<T> Context { get; }

        void Add(IProject project = null);

        bool Remove(IProject project);
    }
}
