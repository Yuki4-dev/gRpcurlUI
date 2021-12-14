using gRpcurlUI.Model;
using System;
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
                OnPropertyChanged(ref _IsSending, value);
                OnPropertyChanged(nameof(SenddingProgressVisible));
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

        public ExecutePageViewModel()
        {
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

        protected abstract void SendCancelExecute();

        protected abstract void SendContentFormatExecute();

        protected abstract void SendExecute();

        public abstract IProject Create(string projectName = null);

        public abstract void Add(IProject project);

        public abstract bool Remove(IProject project);
    }

    public enum ClearArea
    {
        SendContent, Standard, Error
    }
}
