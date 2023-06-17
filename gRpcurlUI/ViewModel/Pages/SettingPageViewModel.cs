using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.API;
using gRpcurlUI.Model.Setting;
using System.Diagnostics;
using System.Threading.Tasks;

namespace gRpcurlUI.ViewModel.Pages
{
    [ObservableObject]
    public partial class SettingPageViewModel
    {
        [ObservableProperty]
        private FontSettingGroup fontSetting;

        [ObservableProperty]
        private BrushSettingGroup brushSetting;

        [ObservableProperty]
        private GrpcurlSettingGroup grpcurlSetting;

        [ObservableProperty]
        private bool isResetEnable = false;

        private readonly IWindowService windowService;

        public SettingPageViewModel(IWindowService windowService, FontSettingGroup fontSetting, BrushSettingGroup brushSetting, GrpcurlSettingGroup grpcurlSetting)
        {
            this.windowService = windowService;
            this.fontSetting = fontSetting;
            this.brushSetting = brushSetting;
            this.grpcurlSetting = grpcurlSetting;
        }

        [RelayCommand]
        private void Back()
        {
            _ = windowService.NavigatePageAsync(NavigatePageType.Back);
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
