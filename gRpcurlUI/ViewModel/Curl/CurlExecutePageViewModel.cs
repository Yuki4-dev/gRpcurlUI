using gRpcurlUI.Core;
using gRpcurlUI.Model;
using gRpcurlUI.Model.Curl;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.ViewModel.Curl
{
    public class CurlExecutePageViewModel : ExecutePageViewModel<CurlProject>
    {
        private readonly CurlProjectContext contextInternal = new CurlProjectContext();
        public override IProjectContext<CurlProject> Context => contextInternal;

        private CurlProject _SelectedProject;
        public override IProject SelectedProject
        {
            get => _SelectedProject;
            set
            {
                OnPropertyChanged(ref _SelectedProject, (CurlProject)value);
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        public CurlExecutePageViewModel(IProcessExecuter executer) : base(executer)
        {
#if DEBUG
            var context = new CurlProject()
            {
                ProjectName = "Sample 1",
                SendContent = "{ \"name\":\"key\"}",
                EndPoint = "localhost:6565",
                Option = "-X GET",
            };
            Add(context);
#endif
        }

        public override void Add(IProject project)
        {
            if (project == null)
            {
                project = new CurlProject()
                {
                    ProjectName = $"Project {DateTime.Now}"
                };
            }

            contextInternal.AddPrject((CurlProject)project);
        }

        public override bool Remove(IProject project)
        {
            return contextInternal.RemovePrject((CurlProject)project);
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

        protected override async Task<IProccesCommand> CreateCommandAsync()
        {
            string content = _SelectedProject.SendContent;
            if (!string.IsNullOrWhiteSpace(SelectedProject.SendContent) && _SelectedProject.IsJsonContent)
            {
                try
                {
                    content = FormatJson(SelectedProject.SendContent);
                }
                catch (Exception ex)
                {
                    var result = await OnShowMessageDialog($"Continue?\r\n\r\nContent is Not Json.\r\n{ex.Message}", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return null;
                    }
                }
            }

            return new CurlCommand(_SelectedProject.Option, _SelectedProject.EndPoint, content);
        }

        private string FormatJson(string json, FormatType formatType = FormatType.None)
        {
            var fomat = formatType == FormatType.None ? Formatting.None : Formatting.Indented;
            var parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, fomat);
        }

    }
}
