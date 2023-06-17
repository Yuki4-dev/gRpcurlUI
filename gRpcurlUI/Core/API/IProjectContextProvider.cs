using gRpcurlUI.Core.Model;

namespace gRpcurlUI.Core.API
{
    public interface IProjectContextProvider
    {
        public IProjectContext[] GetProjectContexts();
    }
}
