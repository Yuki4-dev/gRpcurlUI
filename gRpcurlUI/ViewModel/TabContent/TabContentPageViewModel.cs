﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.ViewModel.TabContent
{
    [ObservableObject]
    public partial class TabContentPageViewModel
    {
        [ObservableProperty]
        private IProjectContext? projectContext;
        partial void OnProjectContextChanging(IProjectContext? value)
        {
            requestAreaViewModel.SelectedProject = null;
        }

        [ObservableProperty]
        private bool isRemoveMode = false;
        partial void OnIsRemoveModeChanged(bool value)
        {
            requestAreaViewModel.SelectedProject = null;
        }

        [ObservableProperty]
        private TabContentRequestAreaViewModel requestAreaViewModel;

        [ObservableProperty]
        private TabContentResponseAreaViewModel responseAreaViewModel;

        [ObservableProperty]
        private TabContentErrorAreaViewModel errorAreaViewModel;

        private readonly IWindowService windowService;

        protected readonly IProcessExecuter processExecuter;

        private readonly IProjectDataService projectDataService;

        private CancellationTokenSource? tokenSource = null;

        public TabContentPageViewModel(IWindowService windowService, IProjectDataService projectDataService, IProcessExecuter processExecuter)
        {
            this.windowService = windowService;
            this.projectDataService = projectDataService;
            this.processExecuter = processExecuter;

            requestAreaViewModel = new TabContentRequestAreaViewModel(processExecuter, windowService);
            errorAreaViewModel = new TabContentErrorAreaViewModel(processExecuter, windowService);
            responseAreaViewModel = new TabContentResponseAreaViewModel(processExecuter, windowService);
        }

        [RelayCommand]
        private async Task Export()
        {
            string fileName = "";
            var result = await windowService.ShowFileDialogAsync(
                FileDialogType.Save,
                (d) =>
                {
                    d.Title = "Project Save";
                    d.Filter = projectDataService.SaveFilter;
                },
                (d) =>
                {
                    fileName = d.FileName;
                });

            if (result)
            {
                try
                {
                    projectDataService.Save(ProjectContext!, fileName);
                }
                catch (Exception ex)
                {
                    _ = await windowService.ShowMessageDialogAsync("Error", ex.Message);
                }
            }
        }

        [RelayCommand]
        private async Task Import()
        {
            string fileName = string.Empty;
            var result = await windowService.ShowFileDialogAsync(
                FileDialogType.Open,
                (d) =>
                {
                    d.Title = "Project Open";
                    d.Filter = projectDataService.OpenFilter;
                },
                (d) =>
                {
                    fileName = d.FileName;
                });

            if (result)
            {
                try
                {
                    var loadCtx = (IProjectContext)projectDataService.Load(fileName, ProjectContext!.GetType());
                    if (loadCtx != null)
                    {
                        ProjectContext.Marge(loadCtx);
                    }
                }
                catch (Exception ex)
                {
                    _ = await windowService.ShowMessageDialogAsync("Error", ex.Message);
                }
            }
        }

        [RelayCommand]
        private async Task Remove()
        {
            if (!IsRemoveMode)
            {
                IsRemoveMode = true;
                return;
            }

            if (ProjectContext is null)
            {
                return;
            }

            var selectedProject = ProjectContext.Projects.Where(p => p.IsSelected).ToList();
            if (selectedProject.Count > 0)
            {
                var result = await windowService.ShowMessageDialogAsync("Remove", $"{selectedProject.Count} Project Remove.", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    selectedProject.ForEach(p => ProjectContext.RemoveProject(p));
                }
            }
            IsRemoveMode = false;
        }

        [RelayCommand]
        private void Cancel()
        {
            IsRemoveMode = false;
        }

        [RelayCommand]
        private void Add()
        {
            ProjectContext?.NewProject();
        }
    }
}

