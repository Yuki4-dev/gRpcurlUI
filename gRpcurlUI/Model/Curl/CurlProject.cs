using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Procces;
using Newtonsoft.Json;
using System;
using System.Text;

namespace gRpcurlUI.Model.Curl
{
    public partial class CurlProject : ObservableObject, IProject
    {
        private string _AppPath = "curl";
        public string AppPath
        {
            get => _AppPath;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetProperty(ref _AppPath, value);
                }
            }
        }

        private string _ProjectName = string.Empty;
        public string ProjectName
        {
            get => _ProjectName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetProperty(ref _ProjectName, value);
                }
            }
        }

        private string _EndPoint = string.Empty;
        public string EndPoint
        {
            get => _EndPoint;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    SetProperty(ref _EndPoint, value);
                }
            }
        }

        [ObservableProperty]
        private string option = string.Empty;

        [ObservableProperty]
        private bool isJsonContent = false;

        [ObservableProperty]
        private string sendContent = string.Empty;

        public CurlProject() { }

        public bool PrepareProject(out string message)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(EndPoint))
            {
                sb.AppendLine("EndPoint is Blank.");
            }

            if (!string.IsNullOrWhiteSpace(SendContent) && IsJsonContent)
            {
                try
                {
                    SendContent = FormatJson(SendContent, Formatting.Indented);
                }
                catch (Exception ex)
                {
                    sb.AppendLine(ex.Message);
                }
            }

            message = sb.ToString();
            return sb.Length == 0;
        }

        public IProccesCommand CreateCommand()
        {
            string jsonContent = SendContent;
            if (!string.IsNullOrWhiteSpace(jsonContent) && IsJsonContent)
            {
                try
                {
                    jsonContent = FormatJson(jsonContent);
                }
                catch { }
            }
            return new CurlCommand(AppPath, Option, EndPoint, jsonContent);
        }

        public object Clone()
        {
            return new CurlProject()
            {
                AppPath = AppPath,
                ProjectName = _ProjectName,
                EndPoint = _EndPoint,
                Option = option,
                IsJsonContent = isJsonContent,
            };
        }

        private string FormatJson(string json, Formatting format = Formatting.None)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, format);
        }
    }
}

