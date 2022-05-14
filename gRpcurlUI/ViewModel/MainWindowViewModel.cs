using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Procces;
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

        public MainWindowViewModel(IWindowService windowService)
        {
            this.windowService = windowService;

            var pe = DI.Get<IProcessExecuter>();
            var pds = DI.Get<IProjectDataService>();
            var viewModel = new TabContentPageViewModel(windowService, pe, pds)
            {
                ProjectContext = new CurlProjectContext()
            };
            TabContents.Add(viewModel);

            var viewModel2 = new TabContentPageViewModel(windowService, pe, pds)
            {
                ProjectContext = new GrpcurlProjectContext()
            };
            TabContents.Add(viewModel2);
        }
    }
}
