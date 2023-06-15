using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.API;
using gRpcurlUI.Model.Setting;
using gRpcurlUI.View.Pages;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace gRpcurlUI.ViewModel.Pages
{
    [ObservableObject]
    public partial class SettingPageViewModel
    {
        [ObservableProperty]
        private FontSetting fontSetting;

        [ObservableProperty]
        private BrushSetting brushSetting;

        [ObservableProperty]
        private bool isResetEnable = false;

        private readonly IWindowService windowService;

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
    }
}
