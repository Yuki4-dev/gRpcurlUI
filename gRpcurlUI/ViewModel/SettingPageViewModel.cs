using gRpcurlUI.Model;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;

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
                if (string.IsNullOrEmpty(value) || File.Exists(value))
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

        public string WindowBackground
        {
            get => appSetting.WindowBackground.ToString();
            set
            {
                if (TryColorParse(value,out var color))
                {
                    appSetting.WindowBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string PageBackground
        {
            get => appSetting.PageBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.PageBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string PageForeground
        {
            get => appSetting.PageForeground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.PageForeground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string BorderBrush
        {
            get => appSetting.BorderBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BorderBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string IconBrush
        {
            get => appSetting.IconBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.IconBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string EditAreaBoxBrush
        {
            get => appSetting.EditAreaBoxBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.EditAreaBoxBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string TextBoxSelectBrush
        {
            get => appSetting.TextBoxSelectBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.TextBoxSelectBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string MouseOverBackground
        {
            get => appSetting.MouseOverBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.MouseOverBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedBackground
        {
            get => appSetting.SelectedBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.SelectedBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string ScrolBarTabBrush
        {
            get => appSetting.ScrolBarTabBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.ScrolBarTabBrush = new SolidColorBrush(color);
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

        private bool TryColorParse(string text, out Color color)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                color = new Color();
                return false;
            }
            try
            {
                color = (Color)ColorConverter.ConvertFromString(text);
                return true;
            }
            catch
            {
                color = new Color();
                return false;
            }
        }
    }
}
