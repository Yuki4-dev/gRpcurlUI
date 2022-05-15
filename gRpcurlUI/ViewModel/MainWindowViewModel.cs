using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Model.Curl;
using gRpcurlUI.Model.Grpcurl;
using gRpcurlUI.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.ViewModel
{
    [ObservableObject]
    public partial class MainWindowViewModel
    {
        [ObservableProperty]
        private IList<TabContentPageViewModel> tabContents = new ObservableCollection<TabContentPageViewModel>();

        private readonly IWindowService windowService;

        public MainWindowViewModel(IWindowService windowService, IProjectDataService projectDataService)
        {
            this.windowService = windowService;

            var grpc = new TabContentPageViewModel(windowService, projectDataService)
            {
                ProjectContext = new GrpcurlProjectContext()
            };
            TabContents.Add(grpc);

            var curl = new TabContentPageViewModel(windowService, projectDataService)
            {
                ProjectContext = new CurlProjectContext()
            };
            TabContents.Add(curl);
        }
    }
}
