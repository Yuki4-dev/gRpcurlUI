using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.Converter.Proto.Analyze;
using gRpcurlUI.Core.Converter.Proto.Format;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Service;
using gRpcurlUI.View.Dialog;
using gRpcurlUI.ViewModel.Dialog;
using gRpcurlUI.ViewModel.Proto;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace gRpcurlUI.Model.Grpcurl
{
    public partial class GrpcurlProject : ObservableObject, IProject
    {
        private string _AppPath = "grpcurl.exe";
        public string AppPath
        {
            get => _AppPath;
            set
            {
                if (string.IsNullOrEmpty(value))
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
        private bool isSelected = false;

        [ObservableProperty]
        private string option = string.Empty;

        [ObservableProperty]
        private string service = string.Empty;

        [ObservableProperty]
        private string sendContent = string.Empty;

        [ObservableProperty]
        private bool isReadProtoButtonEnable = true;

        public GrpcurlProject() { }

        public bool PrepareProject(out string message)
        {
            var sb = new StringBuilder();
            if (!File.Exists(AppPath))
            {
                sb.AppendLine($"{AppPath} does Not Exists.");
            }

            if (string.IsNullOrWhiteSpace(EndPoint))
            {
                sb.AppendLine("EndPoint is Blank.");
            }

            if (!string.IsNullOrWhiteSpace(SendContent))
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
            return new GrpcurlCommand(AppPath, Option, EndPoint, jsonContent, Service);
        }

        public object Clone()
        {
            return new GrpcurlProject()
            {
                AppPath = _AppPath,
                ProjectName = _ProjectName,
                EndPoint = _EndPoint,
                Option = option,
                Service = service,
                SendContent = sendContent
            };
        }

        private static string FormatJson(string json, Formatting format = Formatting.None)
        {
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, format);
        }

        [RelayCommand]
        public static void ReadProto()
        {
            var setting = new ProtoImportPageShareSetting();
            WizardDialog.ShowWizard(new IWizardDialogViewModel[]
            {
                new ProtoImportPage1ViewModel(setting, DI.Get<IWindowService>()),
                new ProtoImportPage2ViewModel(setting, DI.Get<ProtoAnalyzeEntry>()),
                new ProtoImportPage3ViewModel(setting, DI.Get<ProtoFormatEntry>()),
            });
        }
    }
}
