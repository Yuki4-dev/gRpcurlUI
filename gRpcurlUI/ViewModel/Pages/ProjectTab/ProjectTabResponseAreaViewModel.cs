using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model.ProjectTab;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.ViewModel.Pages.ProjectTab
{
    [ObservableObject]
    public partial class ProjectTabResponseAreaViewModel : ITextAreaViewModel, IRecipient<ProcessExecutionStatusMessage>
    {
        [ObservableProperty]
        private bool clearResponse = true;

        [ObservableProperty]
        private bool isStandardOutputCopied = false;

        [ObservableProperty]
        private bool isSending = false;

        private readonly TextControlDisplayBuffer standardOutputBuffer = new();
        public string StandardOutput => standardOutputBuffer.DisplayText;
        public int StandardOutputThick => standardOutputBuffer.IsOverDisplay ? 3 : 0;

        private readonly IWindowService windowService;

        public ProjectTabResponseAreaViewModel(IProcessExecuter processExecuter, IWindowService windowService)
        {
            this.windowService = windowService;
            processExecuter.StandardOutputReceive += ProcessExecuter_StandardOutputReceive;
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        private void ProcessExecuter_StandardOutputReceive(string text)
        {
            standardOutputBuffer.AddText(text + Environment.NewLine);
            OnPropertyChanged(nameof(StandardOutput));
            OnPropertyChanged(nameof(StandardOutputThick));
        }

        [RelayCommand]
        public void TextBoxClear()
        {
            standardOutputBuffer.Clear();
            OnPropertyChanged(nameof(StandardOutput));
            OnPropertyChanged(nameof(StandardOutputThick));
        }

        [RelayCommand]
        private async void StandardOutputCopy()
        {
            try
            {
                Clipboard.SetText(standardOutputBuffer.GetRowText());
                IsStandardOutputCopied = true;
                await Task.Delay(1000);
            }
            catch (Exception ex)
            {
                _ = windowService.ShowMessageDialogAsync(Language.Default.Error, ex.Message);
            }
            finally
            {
                IsStandardOutputCopied = false;
            }
        }

        public void Receive(ClearTextBoxMessage message)
        {
            if (message.ClearTextType == ClearTextBoxType.Response)
            {
                TextBoxClear();
            }
        }

        public void Receive(ProcessExecutionStatusMessage message)
        {
            if (message.ExecutionStatus == ProcessExecutionStatus.PreProcess)
            {
                if (ClearResponse)
                {
                    TextBoxClear();
                }
                IsSending = true;
            }
            else
            {
                IsSending = false;
            }
        }

        public string GetText()
        {
            return standardOutputBuffer.GetRowText();
        }
    }
}
