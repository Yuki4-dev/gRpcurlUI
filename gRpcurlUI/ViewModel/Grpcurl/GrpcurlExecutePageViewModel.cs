using gRpcurlUI.Core;
using gRpcurlUI.Model;
using gRpcurlUI.Model.Grpcurl;
using Newtonsoft.Json;
using System;
using System.Threading;
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

        private bool _IsSending = false;
        public new bool IsSending
        {
            get => _IsSending;
            set
            {
                if (SetProperty(ref _IsSending, value))
                {
                    base.IsSending = value;
                    Command.ChangeCanExecute(!value, SendCommand);
                    Command.ChangeCanExecute(value, SendCancelCommand);
                    OnPropertyChanged();
                }
            }
        }

        private readonly IProcessExecuter processExecuter;

        private readonly IReadOnlyAppSetting grpcurlUISetting;

        private CancellationTokenSource tokenSource;

        public GrpcurlExecutePageViewModel(IProcessExecuter executer, IReadOnlyAppSetting Setting)
        {
            processExecuter = executer;
            processExecuter.StanderdOutputRecieve += (data) => StandardOutput = data;
            processExecuter.StanderdErrorRecieve += (data) => StandardError = data;

            grpcurlUISetting = Setting;

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

        public override IProject Create(string projectName = null)
        {
            return new GrpcurlProject()
            {
                ProjectName = string.IsNullOrWhiteSpace(projectName) ? $"Project {DateTime.Now}" : projectName
            };
        }

        public override void Add(IProject project)
        {
            contextInternal.AddPrject((GrpcurlProject)project);
        }

        public override bool Remove(IProject project)
        {
            return contextInternal.RemovePrject((GrpcurlProject)project);
        }

        protected override async void SendExecute()
        {
            if (!PreGrpcExecute(out var message))
            {
                _ = OnShowMessageDialog(message);
                return;
            }

            IsSending = true;

            if (ClearRepsponse)
            {
                TextBoxClearCommandExecute(ClearArea.Standard.ToString());
            }

            await GrpSendExecuteCore();

            IsSending = false;
        }

        private async Task GrpSendExecuteCore()
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
                    var result = await OnShowMessageDialog($"Continue?\r\n\r\nContent is Not Json.\r\n{ex.Message}", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }
            }

            tokenSource = new CancellationTokenSource();
            try
            {
                var cmd = new GrpcurlCommand(grpcurlUISetting.AppPath, _SelectedProject.Option, _SelectedProject.EndPoint, jsonContent, _SelectedProject.Service);
                await processExecuter.ExecuteAysnc(cmd, tokenSource.Token);
            }
            catch (Exception ex)
            {
                _ = OnShowMessageDialog(ex.Message);
            }
            finally
            {
                tokenSource.Dispose();
                tokenSource = null;
            }
        }

        private bool PreGrpcExecute(out string message)
        {
            if (string.IsNullOrWhiteSpace(_SelectedProject.EndPoint))
            {
                message = "EndPoint Is Blank.";
                return false;
            }

            message = string.Empty;
            return true;
        }

        protected override async void SendCancelExecute()
        {
            var result = await OnShowMessageDialog("Cancel Sendding?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tokenSource?.Cancel();
            }
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
