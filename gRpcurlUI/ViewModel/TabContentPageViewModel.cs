using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.Procces;
using gRpcurlUI.Model;
using gRpcurlUI.Service;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace gRpcurlUI.ViewModel
{
    [ObservableObject]
    public partial class TabContentPageViewModel
    {
        private IProjectContext? _ProjectContext;
        public IProjectContext? ProjectContext
        {
            get => _ProjectContext;
            set
            {
                if (SetProperty(ref _ProjectContext, value))
                {
                    SelectedProject = null;
                }
            }
        }

        private bool _IsRemoveMode = false;
        public bool IsRemoveMode
        {
            get => _IsRemoveMode;
            set
            {
                if (SetProperty(ref _IsRemoveMode, value))
                {
                    SelectedProject = null;
                }
            }
        }

        private bool _IsSending = false;
        public bool IsSending
        {
            get => _IsSending;
            set
            {
                if (SetProperty(ref _IsSending, value))
                {
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SenddingProgressVisible));
                    OnPropertyChanged(nameof(IsNotSending));
                }
            }
        }
        public bool IsNotSending => !IsSending;
        public Visibility SenddingProgressVisible => IsSending ? Visibility.Visible : Visibility.Collapsed;

        private readonly StringBuilder _StandardOutput = new();
        public string StandardOutput
        {
            get => _StandardOutput.ToString();
            set
            {
                if (value != null)
                {
                    _StandardOutput.AppendLine(value);
                    OnPropertyChanged(nameof(StandardOutput));
                }
            }
        }

        private readonly StringBuilder _StandardError = new();
        public string StandardError
        {
            get => _StandardError.ToString();
            set
            {
                if (value != null)
                {
                    _StandardError.AppendLine(value);
                    OnPropertyChanged(nameof(StandardError));
                }
            }
        }

        [ObservableProperty]
        private bool clearRepsponse = true;

        [ObservableProperty]
        private IProject? selectedProject = null;

        private readonly IWindowService windowService;

        protected readonly IProcessExecuter processExecuter;

        private readonly IProjectDataService projectDataService;

        private CancellationTokenSource? tokenSource;

        public TabContentPageViewModel(IWindowService windowService, IProjectDataService projectDataService) : this(windowService, projectDataService, new ProcessExecuter()) { }

        public TabContentPageViewModel(IWindowService windowService, IProjectDataService projectDataService, IProcessExecuter processExecuter)
        {
            this.processExecuter = processExecuter;
            this.windowService = windowService;
            this.projectDataService = projectDataService;

            this.processExecuter.StanderdOutputRecieve += (data) => StandardOutput = data;
            this.processExecuter.StanderdErrorRecieve += (data) => StandardError = data;
        }

        [ICommand]
        private async Task Export()
        {
            string fileName = "";
            var result = await windowService.ShowCommonDialogAsync<SaveFileDialog>(
                (d) =>
                {
                    d.Title = "Project Save";
                    d.Filter = projectDataService.SaveFileter;
                },
                (d) =>
                {
                    fileName = d.FileName;
                });

            if (result)
            {
                try
                {
                    projectDataService.Save(ProjectContext, fileName);
                }
                catch (Exception ex)
                {
                    await windowService.ShowMessageDialogAsync("Error", ex.Message);
                }
            }
        }

        [ICommand]
        private async Task Import()
        {
            string fileName = string.Empty;
            var result = await windowService.ShowCommonDialogAsync<OpenFileDialog>(
                (d) =>
                {
                    d.Title = "Project Open";
                    d.Filter = projectDataService.OpenFileter;
                },
                (d) =>
                {
                    fileName = d.FileName;
                });

            if (result)
            {
                try
                {
                    var Context = (IProjectContext)projectDataService.Load(fileName, ProjectContext.GetType());
                    ProjectContext.Marge(Context);
                }
                catch (Exception ex)
                {
                    await windowService.ShowMessageDialogAsync("Error", ex.Message);
                }
            }
        }

        [ICommand]
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
                var result = await windowService.ShowMessageDialogAsync("Remove", $"{selectedProject.Count} Peoject Remove.", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    selectedProject.ForEach(p => ProjectContext.RemoveProject(p));
                }
            }
            IsRemoveMode = false;
        }

        [ICommand]
        private void Cancel()
        {
            IsRemoveMode = false;
        }

        [ICommand]
        private void Add()
        {
            ProjectContext?.AddProject();
        }

        [ICommand]
        private async void Send()
        {
            if (SelectedProject is null)
            {
                await windowService.ShowMessageDialogAsync("Error", "Project is Nothing.");
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
            if (ClearRepsponse)
            {
                TextBoxClear("2");
            }

            try
            {
                tokenSource = new CancellationTokenSource();
                await processExecuter.ExecuteAysnc(SelectedProject.CreateCommand(), tokenSource.Token);
            }
            catch (Exception ex)
            {
                await windowService.ShowMessageDialogAsync("Error", ex.Message);
            }
            finally
            {
                tokenSource?.Dispose();
                tokenSource = null;
                IsSending = false;
            }
        }

        [ICommand]
        private async void SendCancel()
        {
            var result = await windowService.ShowMessageDialogAsync("Send", "Cancel Sendding?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                tokenSource?.Cancel();
            }
        }

        [ICommand]
        private void SendContentFormat()
        {
            if (SelectedProject is null)
            {
                return;
            }

            if (!SelectedProject.PrepareProject(out var message))
            {
                windowService.ShowMessageDialogAsync("Error", message);
            }
        }

        [ICommand]
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
                    _StandardOutput.Clear();
                    OnPropertyChanged(nameof(StandardOutput));
                    break;
                case "3":
                    _StandardError.Clear();
                    OnPropertyChanged(nameof(StandardError));
                    break;
            }
        }
    }
}

