using gRpcurlUI.Core;
using gRpcurlUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace gRpcurlUI.ViewModel
{
    public class ExecutePageViewModel : ViewModelBase
    {
        private IProject _SelectedProject;
        public IProject SelectedProject
        {
            get => _SelectedProject;
            set
            {
                OnPropertyChanged(ref _SelectedProject, value);
                OnPropertyChanged(nameof(IsEnabled));
            }
        }
        public bool IsEnabled => SelectedProject != null;

        private readonly StringBuilder standerdOutputStringBuilder = new StringBuilder();
        public string StandardOutput
        {
            get => standerdOutputStringBuilder.ToString();
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    standerdOutputStringBuilder.AppendLine(value);
                    OnPropertyChanged();
                }
            }
        }

        private readonly StringBuilder standardErrorStringBuilder = new StringBuilder();
        public string StandardError
        {
            get => standardErrorStringBuilder.ToString();
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    standardErrorStringBuilder.AppendLine(value);
                    OnPropertyChanged();
                }
            }
        }

        public string Encode
        {
            get => processExecuter.Encoding.BodyName;
            set
            {
                try
                {
                    processExecuter.Encoding = Encoding.GetEncoding(value);
                    OnPropertyChanged();
                }
                catch (Exception ex)
                {
                    OnShowMessageDialog(ex.Message);
                }
            }
        }

        private bool _ClearRepsponse = true;
        public bool ClearRepsponse
        {
            get => _ClearRepsponse;
            set => OnPropertyChanged(ref _ClearRepsponse, value);
        }

        private bool _IsSending = false;
        public bool IsSending
        {
            get => _IsSending;
            set
            {
                if (SetProperty(ref _IsSending, value))
                {
                    Command.ChangeCanExecute(!value, SendCommand);
                    Command.ChangeCanExecute(value, SendCancelCommand);
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SenddingProgressVisible));
                }
            }
        }
        public Visibility SenddingProgressVisible => IsSending ? Visibility.Visible : Visibility.Collapsed;

        public ICommand SendCommand { get; }

        public ICommand SendCancelCommand { get; }

        public ICommand SendContentFormatCommand { get; }

        public ICommand TextBoxClearCommand { get; }

        protected readonly IProcessExecuter processExecuter;

        private CancellationTokenSource tokenSource;

        public ExecutePageViewModel() : this(new ProcessExecuter()) { }

        public ExecutePageViewModel(IProcessExecuter executer)
        {
            processExecuter = executer;
            processExecuter.StanderdOutputRecieve += (data) => StandardOutput = data;
            processExecuter.StanderdErrorRecieve += (data) => StandardError = data;

            SendCommand = new Command(SendExecuteAsync);
            SendCancelCommand = new Command(SendCancelExecute) { CanExecuteValue = false };
            SendContentFormatCommand = new Command(SendContentFormatExecute);
            TextBoxClearCommand = Command.Create<string>(TextBoxClearCommandExecute);
        }

        private void TextBoxClearCommandExecute(string type)
        {
            switch (type)
            {
                case "1":
                    SelectedProject.SendContent = "";
                    break;
                case "2":
                    standerdOutputStringBuilder.Clear();
                    OnPropertyChanged(nameof(StandardOutput));
                    break;
                case "3":
                    standardErrorStringBuilder.Clear();
                    OnPropertyChanged(nameof(StandardError));
                    break;
            }
        }

        private async void SendExecuteAsync()
        {
            if (!SelectedProject.PrepareProject(out var message))
            {
                var result = await OnShowMessageDialog("continue?\r\n" + message, MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            IsSending = true;
            if (ClearRepsponse)
            {
                TextBoxClearCommandExecute("2");
            }

            try
            {
                tokenSource = new CancellationTokenSource();
                await processExecuter.ExecuteAysnc(SelectedProject.CreateCommand(), tokenSource.Token);
            }
            catch (Exception ex)
            {
                _ = OnShowMessageDialog(ex.Message);
            }
            finally
            {
                tokenSource?.Dispose();
                tokenSource = null;
                IsSending = false;
            }
        }

        private async void SendCancelExecute()
        {
            var result = await OnShowMessageDialog("Cancel Sendding?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tokenSource?.Cancel();
            }
        }

        private void SendContentFormatExecute()
        {
            if (!SelectedProject.PrepareProject(out var message))
            {
                OnShowMessageDialog(message);
            }
        }
    }
}
