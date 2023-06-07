using gRpcurlUI.Core.API;
using gRpcurlUI.Model;
using Newtonsoft.Json;
using System;
using System.IO;

namespace gRpcurlUI.Service
{
    public class ProjectDataService : IProjectDataService
    {
        public string OpenFilter => "json(*.Json;*.json)|*.Json;*.json";

        public string SaveFilter => "json(*.json)|*.json";

        public IProjectContext? Load(string path, Type type)
        {
            var json = File.ReadAllText(path);
            return (IProjectContext?)JsonConvert.DeserializeObject(json, type);
        }

        public void Save(IProjectContext context, string path)
        {
            var json = JsonConvert.SerializeObject(context.ToJsonObject(), Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
