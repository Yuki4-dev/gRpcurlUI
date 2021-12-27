
using gRpcurlUI.Core;
using Newtonsoft.Json;
using System;
using System.Text;

namespace gRpcurlUI.Model.Curl
{
    public class CurlProject : Observable, IProject
    {
        private string _AppPath = "curl";
        public string AppPath
        {
            get => _AppPath;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    OnPropertyChanged(ref _AppPath, value);
                }
            }
        }

        private string _ProjectName = "";
        public string ProjectName
        {
            get => _ProjectName;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    OnPropertyChanged(ref _ProjectName, value);
                }
            }
        }

        private string _EndPoint = "";
        public string EndPoint
        {
            get => _EndPoint;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    OnPropertyChanged(ref _EndPoint, value);
                }
            }
        }

        private string _Option = "";
        public string Option
        {
            get => _Option;
            set => OnPropertyChanged(ref _Option, value?.Trim());
        }

        private bool _IsJsonContent = false;
        public bool IsJsonContent
        {
            get => _IsJsonContent;
            set => OnPropertyChanged(ref _IsJsonContent, value);
        }

        private string _SendContent = "";
        public string SendContent
        {
            get => _SendContent;
            set => OnPropertyChanged(ref _SendContent, value);
        }

        public CurlProject() { }

        public bool PrepareProject(out string message)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(EndPoint))
            {
                sb.AppendLine("EndPoint Is Blank.");
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
                Option = _Option,
                IsJsonContent = _IsJsonContent,
            };
        }

        private string FormatJson(string json, Formatting format = Formatting.None)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, format);
        }
    }
}

