using gRpcurlUI.Core.Process;
using System.ComponentModel;

namespace gRpcurlUI.Core.Model
{
    public interface IProject : IJsonObject, INotifyPropertyChanged
    {
        bool IsSelected { get; set; }

        string ProjectName { get; set; }

        string SendContent { get; set; }

        bool PrepareProject(out string message);

        IProcessCommand CreateCommand();
    }
}
