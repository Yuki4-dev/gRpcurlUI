using gRpcurlUI.Core.Model;
using gRpcurlUI.Core.Process;
using System;
using System.ComponentModel;

namespace gRpcurlUI.Model
{
    public interface IProject : ICloneable, IJsonObject, INotifyPropertyChanged, INotifyPropertyChanging
    {
        bool IsSelected { get; set; }

        string AppPath { get; set; }

        string ProjectName { get; set; }

        string SendContent { get; set; }

        bool PrepareProject(out string message);

        IProcessCommand CreateCommand();
    }
}
