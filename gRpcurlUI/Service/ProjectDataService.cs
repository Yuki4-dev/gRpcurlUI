using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRpcurlUI.Service
{
    public class ProjectDataService : IProjectDataService
    {
        public string OpenFileter => "json(*.Json;*.json)|*.Json;*.json";

        public string SaveFileter => "json(*.json)|*.json";

        public object Load(string path, Type type)
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject(json, type) ?? string.Empty;
        }

        public void Save<T>(T content, string path)
        {
            var json = JsonConvert.SerializeObject(content, Formatting.Indented);
            File.WriteAllText(path, json);
        }
    }
}
