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
            get => appSetting.BrushSetting.WindowBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.WindowBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string PageBackground
        {
            get => appSetting.BrushSetting.PageBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.PageBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string PageForeground
        {
            get => appSetting.BrushSetting.PageForeground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.PageForeground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string BorderBrush
        {
            get => appSetting.BrushSetting.BorderBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.BorderBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string IconBrush
        {
            get => appSetting.BrushSetting.IconBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.IconBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string EditAreaBoxBrush
        {
            get => appSetting.BrushSetting.EditAreaBoxBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.EditAreaBoxBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string TextBoxSelectBrush
        {
            get => appSetting.BrushSetting.TextBoxSelectBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.TextBoxSelectBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string MouseOverBackground
        {
            get => appSetting.BrushSetting.MouseOverBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.MouseOverBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedBackground
        {
            get => appSetting.BrushSetting.SelectedBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.SelectedBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string ScrolBarTabBrush
        {
            get => appSetting.BrushSetting.ScrolBarTabBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    appSetting.BrushSetting.ScrolBarTabBrush = new SolidColorBrush(color);
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
