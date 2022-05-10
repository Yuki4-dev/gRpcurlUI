using gRpcurlUI.Core.Procces;
using System;
using System.ComponentModel;

namespace gRpcurlUI.Model
{
    public interface IProject : ICloneable, INotifyPropertyChanged, INotifyPropertyChanging
    {
        string AppPath { get; set; }

        string ProjectName { get; set; }

        string SendContent { get; set; }

        bool PrepareProject(out string message);

        IProccesCommand CreateCommand();
    }
}
