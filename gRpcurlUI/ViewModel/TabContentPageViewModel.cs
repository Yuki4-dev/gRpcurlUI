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

namespace gRpcurlUI.ViewModel
{
    [ObservableObject]
    public partial class TabContentPageViewModel
    {
        [ObservableProperty]
        private bool clearResponse = true;

        [ObservableProperty]
        private IProject? selectedProject = null;

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

        [ObservableProperty]
        private bool isSending = false;
        partial void OnIsSendingChanged(bool value)
        {
            OnPropertyChanged(nameof(SendingProgressVisible));
            OnPropertyChanged(nameof(IsNotSending));
        }
        public bool IsNotSending => !IsSending;
        public Visibility SendingProgressVisible => IsSending ? Visibility.Visible : Visibility.Collapsed;

        private readonly TextControlDisplayBuffer standardOutputBuffer = new();
        public string StandardOutput => standardOutputBuffer.DisplayText;

        private readonly TextControlDisplayBuffer standardErrorBuffer = new();
        public string StandardError => standardErrorBuffer.DisplayText;

        private readonly IWindowService windowService;

        protected readonly IProcessExecuter processExecuter;

        private readonly IProjectDataService projectDataService;

        private CancellationTokenSource? tokenSource;

        public TabContentPageViewModel(IWindowService windowService, IProjectDataService projectDataService, IProcessExecuter processExecuter)
        {
            this.processExecuter = processExecuter;
            this.windowService = windowService;
            this.projectDataService = projectDataService;

            this.processExecuter.StandardOutputReceive += (data) => AddStandardOutputBuffer(data);
            this.processExecuter.StandardErrorReceive += (data) => AddStandardErrorBuffer(data);
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
            var sb = new StringBuilder();
            for (int i = 0; i < 2000; i++)
            {
                _ = sb.Append(i.ToString());
            }
            AddStandardOutputBuffer(sb.ToString() + "a");
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

            IsSending = true;
            if (ClearResponse)
            {
                TextBoxClear("2");
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
                IsSending = false;
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
                case "2":
                    standardOutputBuffer.Clear();
                    OnPropertyChanged(nameof(StandardOutput));
                    break;
                case "3":
                    standardErrorBuffer.Clear();
                    OnPropertyChanged(nameof(StandardError));
                    break;
            }
        }

        private void AddStandardOutputBuffer(string text)
        {
            standardOutputBuffer.AddText(text);
            OnPropertyChanged(nameof(StandardOutput));
        }

        private void AddStandardErrorBuffer(string text)
        {
            standardErrorBuffer.AddText(text);
            OnPropertyChanged(nameof(StandardError));
        }
    }
}

