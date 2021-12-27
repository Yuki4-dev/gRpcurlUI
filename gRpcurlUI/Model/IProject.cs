using gRpcurlUI.Core;
using System;
using System.ComponentModel;

namespace gRpcurlUI.Model
{
    public interface IProject : ICloneable, INotifyPropertyChanged
    {
        string AppPath { get; set; }

        string ProjectName { get; set; }

        string SendContent { get; set; }

        bool PrepareProject(out string message);

        IProccesCommand CreateCommand();
    }
}
