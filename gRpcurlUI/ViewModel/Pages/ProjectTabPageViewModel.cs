using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.ViewModel.Pages.TabContent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.ViewModel.Pages
{
    [ObservableObject]
    public partial class ProjectTabPageViewModel
    {
        [ObservableProperty]
        private ICollection<TabContentPageViewModel> tabContents = new ObservableCollection<TabContentPageViewModel>();

        private readonly IWindowService windowService;

        public ProjectTabPageViewModel(IWindowService windowService, IProjectContextProvider projectContextProvider, IProjectDataService projectDataService, ProcessExecuterFactory processExecuterFactory)
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

        [RelayCommand]
        private async void Setting()
        {
            await windowService.NavigatePageAsync(NavigatePageType.Setting);
        }
    }
}
