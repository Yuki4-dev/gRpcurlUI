using gRpcurlUI.Model;
using System;

namespace gRpcurlUI.Core.API
{
    public interface IProjectDataService
    {
        string OpenFilter { get; }

        string SaveFilter { get; }

        void Save(IProjectContext content, string path);

        IProjectContext? Load(string path, Type type);
    }
}
