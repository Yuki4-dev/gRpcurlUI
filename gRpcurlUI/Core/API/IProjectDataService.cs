using gRpcurlUI.Core.Model;
using System;

namespace gRpcurlUI.Core.API
{
    public interface IProjectDataService
    {
        string OpenFilter { get; }

        string SaveFilter { get; }

        void Save(IJsonObject content, string path);

        IJsonObject Load(string path, Type contentType);
    }
}
