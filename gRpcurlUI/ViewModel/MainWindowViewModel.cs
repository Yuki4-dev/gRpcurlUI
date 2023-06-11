using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.ViewModel.TabContent;
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

        public MainWindowViewModel(IWindowService windowService, IProjectContextProvider projectContextProvider, IProjectDataService projectDataService, ProcessExecuterFactory processExecuterFactory)
        {
            this.windowService = windowService;

            var contexts = projectContextProvider.GetProjectContexts();
            foreach (var context in contexts)
            {
                var viewModel = new TabContentPageViewModel(windowService, projectDataService, processExecuterFactory.Create())
                {
                    ProjectContext = context
                };
                TabContents.Add(viewModel);
            }
        }
    }
}
