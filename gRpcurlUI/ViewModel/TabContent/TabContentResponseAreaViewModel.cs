using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model.TabContent;
using gRpcurlUI.Service;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.ViewModel.TabContent
{
    [ObservableObject]
    public partial class TabContentResponseAreaViewModel : ITextAreaViewModel
    {
        [ObservableProperty]
        private bool clearResponse = true;

        [ObservableProperty]
        private bool isStandardOutputCopied = false;

        [ObservableProperty]
        private bool isSending = false;
        partial void OnIsSendingChanged(bool value)
        {
            OnPropertyChanged(nameof(SendingProgressVisible));
            OnPropertyChanged(nameof(IsNotSending));
        }
        public bool IsNotSending => !IsSending;
        public Visibility SendingProgressVisible => IsSending ? Visibility.Visible : Visibility.Collapsed;

        private readonly TextControlDisplayBuffer standardOutputBuffer = new();
        public string StandardOutput => standardOutputBuffer.DisplayText;
        public int StandardOutputThick => standardOutputBuffer.IsOverDisplay ? 3 : 0;

        private readonly IWindowService windowService;

        public TabContentResponseAreaViewModel(IProcessExecuter processExecuter, IWindowService windowService)
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
        private void TextBoxClear()
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
                _ = windowService.ShowMessageDialogAsync("Error", ex.Message);
            }
            finally
            {
                IsStandardOutputCopied = false;
            }
        }

        public void Receive(ClearTextBoxMessage message)
        {
            if (message.ClearTextType == ClearTextBoxType.Response
                || (ClearResponse && message.ClearTextType == ClearTextBoxType.Process))
            {
                standardOutputBuffer.Clear();
            }
        }

        public string GetText()
        {
            return standardOutputBuffer.GetRowText();
        }
    }
}
