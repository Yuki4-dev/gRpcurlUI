using gRpcurlUI.Core;
using gRpcurlUI.Model;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace gRpcurlUI.ViewModel
{
    public interface IExecutePageViewModel<out T> : IProjectHolder<T> where T : IProject
    {
        string StandardOutput { get; set; }

        string StandardError { get; set; }

        bool ClearRepsponse { get; set; }

        bool IsSending { get; set; }

        Visibility SenddingProgressVisible { get; }

        ICommand SendCommand { get; set; }

        ICommand SendCancelCommand { get; set; }

        ICommand SendContentFormatCommand { get; set; }

        ICommand TextBoxClearCommand { get; set; }
    }

    public abstract class ExecutePageViewModel<T> : ViewModelBase, IExecutePageViewModel<T> where T : IProject
    {
        public abstract IProject SelectedProject { get; set; }

        public bool IsEnabled => SelectedProject != null;

        public abstract IProjectContext<T> Context { get; }

        private string _StandardOutput = "";
        public string StandardOutput
        {
            get => _StandardOutput;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    OnPropertyChanged(ref _StandardOutput, StandardOutput + value + "\r\n");
                }
            }
        }

        private string _StandardError = "";
        public string StandardError
        {
            get => _StandardError;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    OnPropertyChanged(ref _StandardError, StandardError + value + "\r\n");
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
                }
            }
        }
        public Visibility SenddingProgressVisible => IsSending ? Visibility.Visible : Visibility.Collapsed;

        private ICommand _SendCommand;
        public ICommand SendCommand
        {
            get => _SendCommand;
            set => OnPropertyChanged(ref _SendCommand, value);
        }

        private ICommand _SendCancelCommand;
        public ICommand SendCancelCommand
        {
            get => _SendCancelCommand;
            set => OnPropertyChanged(ref _SendCancelCommand, value);
        }

        private ICommand _SendContentFormatCommand;
        public ICommand SendContentFormatCommand
        {
            get => _SendContentFormatCommand;
            set => OnPropertyChanged(ref _SendContentFormatCommand, value);
        }

        private ICommand _TextBoxClearCommand;
        public ICommand TextBoxClearCommand
        {
            get => _TextBoxClearCommand;
            set => OnPropertyChanged(ref _TextBoxClearCommand, value);
        }

        protected readonly IProcessExecuter processExecuter;

        protected readonly IReadOnlyAppSetting appSetting;

        protected CancellationTokenSource tokenSource;

        public ExecutePageViewModel(IProcessExecuter executer, IReadOnlyAppSetting Setting)
        {
            processExecuter = executer;
            processExecuter.StanderdOutputRecieve += (data) => StandardOutput = data;
            processExecuter.StanderdErrorRecieve += (data) => StandardError = data;
            appSetting = Setting;

            SendCommand = new Command(SendExecute);
            SendCancelCommand = new Command(SendCancelExecute) { CanExecuteValue = false };
            SendContentFormatCommand = new Command(SendContentFormatExecute);
            TextBoxClearCommand = Command.Create<string>(TextBoxClearCommandExecute);
        }

        protected virtual void TextBoxClearCommandExecute(string type)
        {
            var area = Enum.Parse<ClearArea>(type);
            switch (area)
            {
                case ClearArea.SendContent:
                    SelectedProject.SendContent = "";
                    break;
                case ClearArea.Standard:
                    _StandardOutput = "";
                    OnPropertyChanged(nameof(StandardOutput));
                    break;
                case ClearArea.Error:
                    _StandardError = "";
                    OnPropertyChanged(nameof(StandardError));
                    break;
            }
        }

        protected async void SendExecute()
        {
            if (!PreSending(out var message))
            {
                _ = OnShowMessageDialog(message);
                return;
            }

            IsSending = true;

            if (ClearRepsponse)
            {
                TextBoxClearCommandExecute(ClearArea.Standard.ToString());
            }

            await SendExecuteCore();

            IsSending = false;
        }

        private async Task SendExecuteCore()
        {
            tokenSource = new CancellationTokenSource();
            try
            {
                var cmd = await CreateCommandAsync();
                if (cmd != null)
                {
                    await processExecuter.ExecuteAysnc(cmd, tokenSource.Token);
                }
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

        protected async void SendCancelExecute()
        {
            var result = await OnShowMessageDialog("Cancel Sendding?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tokenSource?.Cancel();
            }
        }

        protected abstract bool PreSending(out string message);

        protected abstract Task<IProccesCommand> CreateCommandAsync();

        protected abstract void SendContentFormatExecute();

        public abstract IProject Create(string projectName = null);

        public abstract void Add(IProject project);

        public abstract bool Remove(IProject project);
    }

    public enum ClearArea
    {
        SendContent, Standard, Error
    }
}
