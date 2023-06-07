using gRpcurlUI.Model;

namespace gRpcurlUI.Core.API
{
    public interface IProjectContextProvider
    {
        public IProjectContext[] GetProjectContexts();
    }
}
