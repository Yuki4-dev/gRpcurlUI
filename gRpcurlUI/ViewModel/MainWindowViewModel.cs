using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.View;
using gRpcurlUI.ViewModel.Pages.TabContent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
