using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model;
using gRpcurlUI.Model.TabContent;
using gRpcurlUI.Service;
using System;
using System.Threading;
using System.Windows;

namespace gRpcurlUI.ViewModel.TabContent
{
    [ObservableObject]
    public partial class TabContentRequestAreaViewModel : ITextAreaViewModel
    {
        [ObservableProperty]
        private IProject? selectedProject = null;

        private readonly IWindowService windowService;

        protected readonly IProcessExecuter processExecuter;

        private CancellationTokenSource? tokenSource;

        public TabContentRequestAreaViewModel(IProcessExecuter processExecuter, IWindowService windowService)
        {
            this.windowService = windowService;
            this.processExecuter = processExecuter;
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        [RelayCommand]
        private void TextBoxClear()
        {
            if (SelectedProject != null)
            {
                SelectedProject.SendContent = string.Empty;
            }
        }

        [RelayCommand]
        private void SendContentFormat()
        {
            if (SelectedProject is null)
            {
                return;
            }

            if (!SelectedProject.PrepareProject(out var message))
            {
                _ = windowService.ShowMessageDialogAsync("Error", message);
            }
        }

        [RelayCommand]
        private async void Send()
        {
            if (SelectedProject is null)
            {
                _ = await windowService.ShowMessageDialogAsync("Error", "Project is Nothing.");
                return;
            }

            if (!SelectedProject.PrepareProject(out var message))
            {
                var result = await windowService.ShowMessageDialogAsync("Send", "continue?\r\n" + message, MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            WeakReferenceMessenger.Default.Send(new ProcessExecutionStatusMessage(ProcessExecutionStatus.PreProcess));
            try
            {
                tokenSource = new CancellationTokenSource();
                await processExecuter.ExecuteAsync(SelectedProject.CreateCommand(), tokenSource.Token);
            }
            catch (Exception ex)
            {
                _ = await windowService.ShowMessageDialogAsync("Error", ex.Message);
                WeakReferenceMessenger.Default.Send(new ProcessExecutionStatusMessage(ProcessExecutionStatus.Error));
            }
            finally
            {
                tokenSource?.Dispose();
                tokenSource = null;
                WeakReferenceMessenger.Default.Send(new ProcessExecutionStatusMessage(ProcessExecutionStatus.PostProcess));
            }
        }

        [RelayCommand]
        private async void SendCancel()
        {
            var result = await windowService.ShowMessageDialogAsync("Send", "Cancel Sending?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tokenSource?.Cancel();
            }
        }

        public void Receive(ClearTextBoxMessage message)
        {
            if (message.ClearTextType == ClearTextBoxType.Request)
            {
                TextBoxClear();
            }
        }

        public string GetText()
        {
            return SelectedProject?.SendContent ?? string.Empty;
        }
    }
}
