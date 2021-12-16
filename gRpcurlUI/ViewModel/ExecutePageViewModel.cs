using gRpcurlUI.Core;
using gRpcurlUI.Model;
using System;
using System.Text;
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

        public bool IsEnabled => SelectedProject != null;

        public abstract IProject SelectedProject { get; set; }

        public abstract IProjectContext<T> Context { get; }

        protected readonly IProcessExecuter processExecuter;

        private CancellationTokenSource tokenSource;

        public ExecutePageViewModel(IProcessExecuter executer)
        {
            processExecuter = executer;
            processExecuter.StanderdOutputRecieve += (data) => StandardOutput = data;
            processExecuter.StanderdErrorRecieve += (data) => StandardError = data;

            SetCommand();
        }

        private void SetCommand()
        {
            SendCommand = new Command(SendExecuteAsync);
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
                    standerdOutputStringBuilder.Clear();
                    OnPropertyChanged(nameof(StandardOutput));
                    break;
                case ClearArea.Error:
                    standardErrorStringBuilder.Clear();
                    OnPropertyChanged(nameof(StandardError));
                    break;
            }
        }

        protected virtual async void SendExecuteAsync()
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

            await SendExecuteCoreAsync();

            IsSending = false;
        }

        private async Task SendExecuteCoreAsync()
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

        protected virtual async void SendCancelExecute()
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

        public abstract void Add(IProject project = null);

        public abstract bool Remove(IProject project);
    }

    public enum ClearArea
    {
        SendContent, Standard, Error
    }
}
