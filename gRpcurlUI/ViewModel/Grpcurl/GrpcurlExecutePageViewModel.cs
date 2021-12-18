using gRpcurlUI.Core;
using gRpcurlUI.Model;
using gRpcurlUI.Model.Grpcurl;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.ViewModel.Grpcurl
{
    public class GrpcurlExecutePageViewModel : ExecutePageViewModel<GrpcurlProject>
    {
        private readonly GrpcurlProjectContext contextInternal = new GrpcurlProjectContext();
        public override IProjectContext<GrpcurlProject> Context => contextInternal;

        private GrpcurlProject _SelectedProject;
        public override IProject SelectedProject
        {
            get => _SelectedProject;
            set
            {
                OnPropertyChanged(ref _SelectedProject, (GrpcurlProject)value);
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        protected readonly IReadOnlyAppSetting appSetting;

        public GrpcurlExecutePageViewModel(IProcessExecuter executer, IReadOnlyAppSetting Setting) : base(executer)
        {
            appSetting = Setting;
#if DEBUG
            var context = new GrpcurlProject()
            {
                ProjectName = "Sample 1",
                SendContent = "{ \"name\":\"key\"}",
                EndPoint = "localhost:6565",
                Option = "-plaintext",
                Service = "list"
            };
            Add(context);
#endif
        }

        public override void Add(IProject project = null)
        {
            if (project == null)
            {
                project = new GrpcurlProject()
                {
                    ProjectName = $"Project {DateTime.Now}"
                };
            }

            contextInternal.AddPrject((GrpcurlProject)project);
        }

        public override bool Remove(IProject project)
        {
            return contextInternal.RemovePrject((GrpcurlProject)project);
        }

        protected override async Task<IProccesCommand> CreateCommandAsync()
        {
            string jsonContent = "";
            if (!string.IsNullOrWhiteSpace(SelectedProject.SendContent))
            {
                try
                {
                    jsonContent = FormatJson(SelectedProject.SendContent);
                }
                catch (Exception ex)
                {
                    var result = await OnShowMessageDialog($"Continue?\r\nContent is Not Json.\r\n{ex.Message}", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return null;
                    }
                }
            }

            return new GrpcurlCommand(appSetting.AppPath, _SelectedProject.Option, _SelectedProject.EndPoint, jsonContent, _SelectedProject.Service);
        }

        protected override bool PreSending(out string message)
        {
            if (string.IsNullOrWhiteSpace(_SelectedProject.EndPoint))
            {
                message = "EndPoint Is Blank.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        protected override void SendContentFormatExecute()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(SelectedProject.SendContent))
                {
                    SelectedProject.SendContent = FormatJson(SelectedProject.SendContent, FormatType.View);
                }
            }
            catch (Exception ex)
            {
                OnShowMessageDialog(ex.Message);
            }
        }

        private string FormatJson(string json, FormatType formatType = FormatType.None)
        {
            var fomat = formatType == FormatType.None ? Formatting.None : Formatting.Indented;
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, fomat);
        }
    }
}
