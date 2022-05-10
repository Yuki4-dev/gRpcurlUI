using System;

namespace gRpcurlUI.Service
{
    public interface IProjectDataService
    {
        string OpenFileter { get; }

        string SaveFileter { get; }

        void Save<T>(T content, string path);

        object Load(string path, Type type);
    }
}
