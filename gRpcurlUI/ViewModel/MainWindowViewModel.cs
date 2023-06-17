using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.API;

namespace gRpcurlUI.ViewModel
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        private readonly IWindowService windowService;

        public MainWindowViewModel(IWindowService windowService)
        {
            this.windowService = windowService;
        }
    }
}
