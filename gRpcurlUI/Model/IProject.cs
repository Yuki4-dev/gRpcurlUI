using gRpcurlUI.Core;
using System;

namespace gRpcurlUI.Model
{
    public interface IProject : ICloneable
    {
        string AppPath { get; set; }

        string ProjectName { get; set; }

        string SendContent { get; set; }

        bool PrepareProject(out string message);

        IProccesCommand CreateCommand();
    }
}
