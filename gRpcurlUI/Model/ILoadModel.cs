using Newtonsoft.Json;
using System;
using System.IO;

namespace gRpcurlUI.Model
{
    public interface ILoadModel
    {
        string OpenFileter { get; }

        string SaveFileter { get; }

        void Save<T>(T content, string path);

        object Load(string path, Type type);
    }

    public class JsonLoadModel : ILoadModel
    {
        public string OpenFileter => "json(*.Json;*.json)|*.Json;*.json";

        public string SaveFileter => "json(*.json)|*.json";

        public object Load(string path, Type type)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject(json, type);
        }

        public void Save<T>(T content, string path)
        {
            var json = JsonConvert.SerializeObject(content, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
