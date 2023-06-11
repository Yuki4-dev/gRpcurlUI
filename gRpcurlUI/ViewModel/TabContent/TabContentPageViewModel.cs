using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Model;
using gRpcurlUI.Model.TabContent;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.ViewModel.TabContent
{
    [ObservableObject]
    public partial class TabContentPageViewModel
    {
        [ObservableProperty]
        private IProject? selectedProject = null;

        [ObservableProperty]
        private TabContentResponseAreaViewModel responseAreaViewModel;

        [ObservableProperty]
        private TabContentErrorAreaViewModel errorAreaViewModel;

        [ObservableProperty]
        private IProjectContext? projectContext;
        partial void OnProjectContextChanging(IProjectContext? value)
        {
            SelectedProject = null;
        }

        [ObservableProperty]
        private bool isRemoveMode = false;
        partial void OnIsRemoveModeChanged(bool value)
        {
            SelectedProject = null;
        }

        private readonly IWindowService windowService;

        protected readonly IProcessExecuter processExecuter;

        private readonly IProjectDataService projectDataService;

        private CancellationTokenSource? tokenSource;

        public TabContentPageViewModel(IWindowService windowService, IProjectDataService projectDataService, IProcessExecuter processExecuter)
        {
            this.windowService = windowService;
            this.projectDataService = projectDataService;

            this.processExecuter = processExecuter;
            errorAreaViewModel = new TabContentErrorAreaViewModel(processExecuter);
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
                    var contextJson = projectDataService.Load(fileName, ProjectContext!.JsonType);
                    if (contextJson != null)
                    {
                        ProjectContext.Marge(ProjectContextFactory.CreateFromJson(ProjectContext.GetType(), contextJson));
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
            ProjectContext?.AddProject();
        }

        [RelayCommand]
        private async void Send()
        {
            if (SelectedProject is null)
            {
                _ = await windowService.ShowMessageDialogAsync("Error", "Project is Nothing.");
                return;
            }

            if (!SelectedProject.PrepareProject(out var message))
            {
                var result = await windowService.ShowMessageDialogAsync("Send", "continue?\r\n" + message, MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            responseAreaViewModel.IsSending = true;
            if (responseAreaViewModel.ClearResponse)
            {
                //
            }

            try
            {
                tokenSource = new CancellationTokenSource();
                await processExecuter.ExecuteAsync(SelectedProject.CreateCommand(), tokenSource.Token);
            }
            catch (Exception ex)
            {
                _ = await windowService.ShowMessageDialogAsync("Error", ex.Message);
            }
            finally
            {
                tokenSource?.Dispose();
                tokenSource = null;
                responseAreaViewModel.IsSending = false;
            }
        }

        [RelayCommand]
        private async void SendCancel()
        {
            var result = await windowService.ShowMessageDialogAsync("Send", "Cancel Sending?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tokenSource?.Cancel();
            }
        }

        [RelayCommand]
        private void SendContentFormat()
        {
            if (SelectedProject is null)
            {
                return;
            }

            if (!SelectedProject.PrepareProject(out var message))
            {
                _ = windowService.ShowMessageDialogAsync("Error", message);
            }
        }

        [RelayCommand]
        private void TextBoxClear(string type)
        {
            switch (type)
            {
                case "1":
                    if (SelectedProject != null)
                    {
                        SelectedProject.SendContent = "";
                    }
                    break;
            }
        }
    }
}

