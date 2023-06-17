using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Model;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model.Setting;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace gRpcurlUI.Model.Grpcurl
{
    public partial class GrpcurlProject : ObservableObject, IProject
    {
        public Type JsonType => typeof(GrpcProjectJson);

        [ObservableProperty]
        private string projectName = string.Empty;

        [ObservableProperty]
        private string endPoint = string.Empty;

        [ObservableProperty]
        private bool isSelected = false;

        [ObservableProperty]
        private string option = string.Empty;

        [ObservableProperty]
        private string service = string.Empty;

        [ObservableProperty]
        private string sendContent = string.Empty;

        [ObservableProperty]
        private bool isReadProtoButtonEnable = true;

        private readonly GrpcurlSettingGroup grpcurlSettingGroup;

        public GrpcurlProject() 
        {
            grpcurlSettingGroup = DI.Get<GrpcurlSettingGroup>();
        }

        public bool PrepareProject(out string message)
        {
            var sb = new StringBuilder();
            if (!File.Exists(grpcurlSettingGroup.ExePath))
            {
                _ = sb.AppendLine($"{grpcurlSettingGroup.ExePath} does Not Exists.");
            }

            if (string.IsNullOrWhiteSpace(EndPoint))
            {
                _ = sb.AppendLine("EndPoint is Blank.");
            }

            if (!string.IsNullOrWhiteSpace(SendContent))
            {
                try
                {
                    SendContent = FormatJson(SendContent, Formatting.Indented);
                }
                catch (Exception ex)
                {
                    _ = sb.AppendLine(ex.Message);
                }
            }

            message = sb.ToString();
            return sb.Length == 0;
        }

        public IProcessCommand CreateCommand()
        {
            string jsonContent = SendContent;
            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                try
                {
                    jsonContent = FormatJson(jsonContent);
                }
                catch { }
            }
            return new GrpcurlCommand(grpcurlSettingGroup.ExePath, Option, EndPoint, jsonContent, Service);
        }

        public object ToJsonObject()
        {
            return new GrpcProjectJson()
            {
                ProjectName = ProjectName,
                EndPoint = EndPoint,
                IsSelected = IsSelected,
                Option = Option,
                SendContent = SendContent,
                Service = Service,
            };
        }

        public void LoadJsonObject(object jsonObject)
        {
            if (jsonObject is not GrpcProjectJson grpcProject)
            {
                throw new Exception("Json is not GrpcProject.");
            }

            ProjectName = grpcProject.ProjectName;
            EndPoint = grpcProject.EndPoint;
            IsSelected = grpcProject.IsSelected;
            Option = grpcProject.Option;
            SendContent = grpcProject.SendContent;
            Service = grpcProject.Service;
        }

        private static string FormatJson(string json, Formatting format = Formatting.None)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, format);
        }

        internal class GrpcProjectJson
        {
            public string ProjectName { get; set; } = string.Empty;
            public string EndPoint { get; set; } = string.Empty;
            public bool IsSelected { get; set; } = false;
            public string Option { get; set; } = string.Empty;
            public string SendContent { get; set; } = string.Empty;
            public string Service { get; set; } = string.Empty;
        }
    }
}
