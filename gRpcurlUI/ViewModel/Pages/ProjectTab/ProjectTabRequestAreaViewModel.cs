using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Model;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model.ProjectTab;
using System;
using System.Threading;
using System.Windows;

namespace gRpcurlUI.ViewModel.Pages.ProjectTab
{
    [ObservableObject]
    public partial class ProjectTabRequestAreaViewModel : ITextAreaViewModel
    {
        [ObservableProperty]
        private IProject? selectedProject = null;

        [ObservableProperty]
        private bool isSending = false;

        [ObservableProperty]
        private DisplayTimer executionTimer = new();

        private readonly IWindowService windowService;

        protected readonly IProcessExecuter processExecuter;

        private CancellationTokenSource? tokenSource;

        public ProjectTabRequestAreaViewModel(IProcessExecuter processExecuter, IWindowService windowService)
        {
            this.windowService = windowService;
            this.processExecuter = processExecuter;
            WeakReferenceMessenger.Default.RegisterAll(this);
        }

        [RelayCommand]
        public void TextBoxClear()
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
                _ = windowService.ShowMessageDialogAsync(Language.Default.Error, message);
            }
        }

        [RelayCommand]
        private async void Send()
        {
            if (SelectedProject is null)
            {
                _ = await windowService.ShowMessageDialogAsync(Language.Default.Error, Language.Default.ProjectTabPage.ProjectNothing);
                return;
            }

            if (!SelectedProject.PrepareProject(out var message))
            {
                var result = await windowService.ShowMessageDialogAsync(Language.Default.ProjectTabPage.Send, Language.Default.ProjectTabPage.ContinueQ + Environment.NewLine + message, MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            IsSending = true;
            _ = WeakReferenceMessenger.Default.Send(new ProcessExecutionStatusMessage(ProcessExecutionStatus.PreProcess));
            try
            {
                tokenSource = new CancellationTokenSource();
                ExecutionTimer.Start();
                await processExecuter.ExecuteAsync(SelectedProject.CreateCommand(), tokenSource.Token);
                ExecutionTimer.Stop();
            }
            catch (Exception ex)
            {
                ExecutionTimer.Stop();
                _ = await windowService.ShowMessageDialogAsync(Language.Default.Error, ex.Message);
                _ = WeakReferenceMessenger.Default.Send(new ProcessExecutionStatusMessage(ProcessExecutionStatus.Error));
            }
            finally
            {
                tokenSource?.Dispose();
                tokenSource = null;
                IsSending = false;
                _ = WeakReferenceMessenger.Default.Send(new ProcessExecutionStatusMessage(ProcessExecutionStatus.Done));
            }
        }

        [RelayCommand]
        private async void SendCancel()
        {
            var result = await windowService.ShowMessageDialogAsync(Language.Default.ProjectTabPage.Send, Language.Default.ProjectTabPage.CancelSendingQ, MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tokenSource?.Cancel();
                _ = WeakReferenceMessenger.Default.Send(new ProcessExecutionStatusMessage(ProcessExecutionStatus.Cancel));
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
