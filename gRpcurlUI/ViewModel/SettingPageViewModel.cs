using gRpcurlUI.Model.Setting;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

namespace gRpcurlUI.ViewModel
{
    public class SettingPageViewModel : ViewModelBase
    {
        private readonly FontSetting fontSetting;

        private readonly BrushSetting brushSetting;

        private bool _IsResetEnable = false;
        public bool IsResetEnable
        {
            get => _IsResetEnable;
            set
            {
                if (SetProperty(ref _IsResetEnable, value))
                {
                    if (_IsResetEnable)
                    {
                        fontSetting.Caputure();
                        brushSetting.Caputure();
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string FontFamily
        {
            get => fontSetting.FontFamily.ToString();
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var font = new FontFamily(value);
                    if (font.FamilyNames.Values.Contains(value))
                    {
                        IsResetEnable = true;
                        fontSetting.FontFamily = font;
                        OnPropertyChanged();
                    }
                }
            }
        }

        public string EditFontSize
        {
            get => fontSetting.EditFontSize.ToString();
            set
            {
                if (int.TryParse(value, out var size)
                    && size > 0)
                {
                    IsResetEnable = true;
                    fontSetting.EditFontSize = (uint)size;
                    OnPropertyChanged();
                }
            }
        }

        public string WindowBackground
        {
            get => brushSetting.WindowBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.WindowBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string PageBackground
        {
            get => brushSetting.PageBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.PageBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string PageForeground
        {
            get => brushSetting.PageForeground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.PageForeground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string BorderBrush
        {
            get => brushSetting.BorderBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.BorderBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string IconBrush
        {
            get => brushSetting.IconBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.IconBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string EditAreaTextBoxBrush
        {
            get => brushSetting.EditAreaTextBoxBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.EditAreaTextBoxBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string TextBoxSelectBrush
        {
            get => brushSetting.TextBoxSelectBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.TextBoxSelectBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string MouseOverBackground
        {
            get => brushSetting.MouseOverBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.MouseOverBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedBackground
        {
            get => brushSetting.SelectedBackground.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.SelectedBackground = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public string ScrolBarTabBrush
        {
            get => brushSetting.ScrolBarTabBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.ScrolBarTabBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ResetCommand { get; }

        public ICommand OpenSourceCommand { get; }

        public ICommand AboutCommand { get; }

        public SettingPageViewModel(FontSetting fontSetting, BrushSetting brushSetting)
        {
            this.fontSetting = fontSetting;
            this.brushSetting = brushSetting;
            ResetCommand = new Command(ResetExecute);
            OpenSourceCommand = new Command(OpenSourceExecute);
            AboutCommand = new Command(AboutExecute);
        }

        private void ResetExecute()
        {
            IsResetEnable = false;
            fontSetting.ResetToCaputure();
            brushSetting.ResetToCaputure();
            foreach (var prop in typeof(SettingPageViewModel).GetProperties())
            {
                OnPropertyChanged(prop.Name);
            }
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
