using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.API;
using gRpcurlUI.Model.Setting;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;

namespace gRpcurlUI.ViewModel.Setting
{
    [ObservableObject]
    public partial class SettingPageViewModel
    {
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

        public string ScrollBarTabBrush
        {
            get => brushSetting.ScrollBarTabBrush.ToString();
            set
            {
                if (TryColorParse(value, out var color))
                {
                    IsResetEnable = true;
                    brushSetting.ScrollBarTabBrush = new SolidColorBrush(color);
                    OnPropertyChanged();
                }
            }
        }

        private readonly IWindowService windowService;

        private readonly FontSetting fontSetting;

        private readonly BrushSetting brushSetting;

        public SettingPageViewModel() : this(DI.Get<IWindowService>(), new FontSetting(App.Current.Resources), new BrushSetting(App.Current.Resources)) { }

        public SettingPageViewModel(IWindowService windowService, FontSetting fontSetting, BrushSetting brushSetting)
        {
            this.windowService = windowService;
            this.fontSetting = fontSetting;
            this.brushSetting = brushSetting;
        }

        [RelayCommand]
        private void Reset()
        {
            IsResetEnable = false;
            fontSetting.ResetToCaputure();
            brushSetting.ResetToCaputure();
            foreach (var prop in typeof(SettingPageViewModel).GetProperties())
            {
                OnPropertyChanged(prop.Name);
            }
        }

        [RelayCommand]
        private async Task About()
        {
            _ = await windowService.ShowMessageDialogAsync("About...", "gRpcurlUI Ver 1.0.0" + "\r\n" + "Preview");
        }

        [RelayCommand]
        private void OpenSource()
        {
            var pi = new ProcessStartInfo()
            {
                FileName = @"https://github.com/Yuki4-dev/gRpcurlUI",
                UseShellExecute = true,
            };

            try
            {
                _ = Process.Start(pi);
            }
            catch { }
        }

        private static bool TryColorParse(string text, out Color color)
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
