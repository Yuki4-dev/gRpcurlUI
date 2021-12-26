
using gRpcurlUI.Core;
using Newtonsoft.Json;
using System;
using System.Text;

namespace gRpcurlUI.Model.Grpcurl
{
    public class GrpcurlProject : Observable, IProject
    {
        private string _AppPath = "grpcurl.exe";
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

        private string _Service = "";
        public string Service
        {
            get => _Service;
            set => OnPropertyChanged(ref _Service, value?.Trim());
        }

        private string _SendContent = "";
        public string SendContent
        {
            get => _SendContent;
            set => OnPropertyChanged(ref _SendContent, value);
        }


        public GrpcurlProject() { }

        public bool PrepareProject(out string message)
        {
            var sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(EndPoint))
            {
                sb.AppendLine("EndPoint Is Blank.");
            }

            if (!string.IsNullOrWhiteSpace(SendContent))
            {
                try
                {
                    SendContent = FormatJson(SendContent, FormatType.View);
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
            return new GrpcurlCommand(AppPath, Option, EndPoint, SendContent, Service);
        }

        public object Clone()
        {
            return new GrpcurlProject()
            {
                AppPath = AppPath,
                ProjectName = _ProjectName,
                EndPoint = _EndPoint,
                Option = _Option,
                Service = _Service
            };
        }

        private string FormatJson(string json, FormatType formatType = FormatType.None)
        {
            var fomat = formatType == FormatType.None ? Formatting.None : Formatting.Indented;
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, fomat);
        }
    }
}
