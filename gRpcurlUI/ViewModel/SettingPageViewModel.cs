using gRpcurlUI.Model;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace gRpcurlUI.ViewModel
{
    public class SettingPageViewModel : ViewModelBase
    {
        private readonly AppSetting appSetting;

        public string AppPath
        {
            get => appSetting.AppPath;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && File.Exists(value))
                {
                    AppPathMessage = "";
                    appSetting.AppPath = value;
                    OnPropertyChanged();
                }
                else
                {
                    AppPathMessage = value + " is Not Found.";
                }
            }
        }

        private string _AppPathMessage;
        public string AppPathMessage
        {
            get => _AppPathMessage;
            set => OnPropertyChanged(ref _AppPathMessage, value);
        }

        public string EditFontSize
        {
            get => appSetting.EditFontSize.ToString();
            set
            {
                if (int.TryParse(value, out var size)
                    && size > 0)
                {
                    appSetting.EditFontSize = (uint)size;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _OpenSourceCommand;
        public ICommand OpenSourceCommand
        {
            get => _OpenSourceCommand;
            set => OnPropertyChanged(ref _OpenSourceCommand, value);
        }

        private ICommand _AboutCommand;
        public ICommand AboutCommand
        {
            get => _AboutCommand;
            set => OnPropertyChanged(ref _AboutCommand, value);
        }

        public SettingPageViewModel(AppSetting setting)
        {
            appSetting = setting;
            OpenSourceCommand = new Command(OpenSourceExecute);
            AboutCommand = new Command(AboutExecute);
        }

        private async void AboutExecute()
        {
            await OnShowMessageDialog("gRpcurlUI Ver 1.0.0" + "\r\n"
                                         + "Preview");
        }

        private void OpenSourceExecute()
        {
            var pi = new ProcessStartInfo()
            {
                FileName = @"https://github.com/Yuki4-dev/gRpcurlUI",
                UseShellExecute = true,
            };

            try
            {
                Process.Start(pi);
            }
            catch { }
        }
    }
}
