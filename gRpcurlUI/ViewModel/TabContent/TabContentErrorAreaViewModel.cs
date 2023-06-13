using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model.TabContent;
using System;

namespace gRpcurlUI.ViewModel.TabContent
{
    [ObservableObject]
    public partial class TabContentErrorAreaViewModel: ITextAreaViewModel
    {
        public TextControlDisplayBuffer standardErrorBuffer = new();
        public string StandardError => standardErrorBuffer.DisplayText;
        public int StandardErrorThick => standardErrorBuffer.IsOverDisplay ? 3 : 0;

        private readonly IWindowService windowService;

        public TabContentErrorAreaViewModel(IProcessExecuter processExecuter, IWindowService windowService)
        {
            this.windowService = windowService;
            processExecuter.StandardErrorReceive += ProcessExecuter_StandardErrorReceive;
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        private void ProcessExecuter_StandardErrorReceive(string text)
        {
            standardErrorBuffer.AddText(text + Environment.NewLine);
            OnPropertyChanged(nameof(StandardError));
            OnPropertyChanged(nameof(StandardErrorThick));
        }

        [RelayCommand]
        public void TextBoxClear()
        {
            standardErrorBuffer.Clear();
            OnPropertyChanged(nameof(StandardError));
            OnPropertyChanged(nameof(StandardErrorThick));
        }

        public void Receive(ClearTextBoxMessage message)
        {
            if(message.ClearTextType == ClearTextBoxType.Error)
            {
                TextBoxClear();
            }
        }

        public string GetText()
        {
            return standardErrorBuffer.GetRowText();
        }
    }
}
