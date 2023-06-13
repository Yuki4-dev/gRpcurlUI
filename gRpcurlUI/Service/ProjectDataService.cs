using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Model;
using Newtonsoft.Json;
using System;
using System.IO;

namespace gRpcurlUI.Service
{
    public class ProjectDataService : IProjectDataService
    {
        public string OpenFilter => "json(*.Json;*.json)|*.Json;*.json";

        public string SaveFilter => "json(*.json)|*.json";

        public IJsonObject Load(string path, Type contentType)
        {
            if (contentType.IsSubclassOf(typeof(IJsonObject)))
            {
                throw new Exception($"{nameof(contentType)} is Not {typeof(IJsonObject).FullName}.");
            }

            var json = File.ReadAllText(path);
            if (json == null || string.IsNullOrWhiteSpace(json))
            {
                throw new Exception($"{path} is Empty.");
            }

            var context = (IJsonObject?)Activator.CreateInstance(contentType);
            var jsonObj = JsonConvert.DeserializeObject(json, context!.JsonType)!;
            context.LoadJsonObject(jsonObj);
            return context;
        }

        public void Save(IJsonObject context, string path)
        {
            var json = JsonConvert.SerializeObject(context.ToJsonObject(), Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
