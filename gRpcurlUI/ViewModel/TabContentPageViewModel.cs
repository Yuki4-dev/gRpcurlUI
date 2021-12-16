using gRpcurlUI.Core;
using gRpcurlUI.Model;
using gRpcurlUI.ViewModel.Curl;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace gRpcurlUI.ViewModel
{
    public class TabContentPageViewModel : ViewModelBase
    {
        private IExecutePageViewModel<IProject> _ExcutePageViewModel;
        public IExecutePageViewModel<IProject> ExecutePageViewModel
        {
            get => _ExcutePageViewModel;
            set => OnPropertyChanged(ref _ExcutePageViewModel, value);
        }

        private SettingPageViewModel _SettingPageViewModel;
        public SettingPageViewModel SettingPageViewModel
        {
            get => _SettingPageViewModel;
            set => OnPropertyChanged(ref _SettingPageViewModel, value);
        }

        private bool _IsRemoveMode;
        public bool IsRemoveMode
        {
            get => _IsRemoveMode;
            set
            {
                if (SetProperty(ref _IsRemoveMode, value))
                {
                    removeProject.Clear();
                    SelectedDefault(null);
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _SelectedCommand;
        public ICommand SelectedCommand
        {
            get => _SelectedCommand;
            set => OnPropertyChanged(ref _SelectedCommand, value);
        }

        private ICommand _AddCommand;
        public ICommand AddCommand
        {
            get => _AddCommand;
            set => OnPropertyChanged(ref _AddCommand, value);
        }

        private ICommand _RemoveCommand;
        public ICommand RemoveCommand
        {
            get => _RemoveCommand;
            set => OnPropertyChanged(ref _RemoveCommand, value);
        }

        private ICommand _ImportCommand;
        public ICommand ImportCommand
        {
            get => _ImportCommand;
            set => OnPropertyChanged(ref _ImportCommand, value);
        }

        private ICommand _ExportCommand;
        public ICommand ExportCommand
        {
            get => _ExportCommand;
            set => OnPropertyChanged(ref _ExportCommand, value);
        }

        private ICommand _CancelCommand;
        public ICommand CancelCommand
        {
            get => _CancelCommand;
            set => OnPropertyChanged(ref _CancelCommand, value);
        }

        private readonly ILoadModel loadModel = new LoadModel();

        private readonly List<IProject> removeProject = new List<IProject>();

        public TabContentPageViewModel()
        {
            var setting = new AppSetting(App.Current.Resources);
            SettingPageViewModel = new SettingPageViewModel(setting);
            ExecutePageViewModel = new CurlExecutePageViewModel(new ProcessExecuter());
#if DEBUG
            setting.AppPath = "grpcurl.exe";
#endif
            SetCommand();
        }

        public TabContentPageViewModel(ILoadModel load, IExecutePageViewModel<IProject> holder, AppSetting setting)
        {
            loadModel = load;
            ExecutePageViewModel = holder;
            SettingPageViewModel = new SettingPageViewModel(setting);

            SetCommand();
        }

        private void SetCommand()
        {
            SelectedCommand = Command.Create<IProject>(SelectedExecute);
            AddCommand = new Command(AddExecute);
            RemoveCommand = new Command(RemoveExecutAsynce);
            ImportCommand = new Command(ImportExecuteAsync);
            ExportCommand = new Command(ExportExecuteAsync);
            CancelCommand = new Command(CancelExecute);
        }

        private void SelectedExecute(IProject project)
        {
            if (IsRemoveMode)
            {
                SelectedRemove(project);
            }
            else
            {
                SelectedDefault(project);
            }
        }

        private async void RemoveExecutAsynce()
        {
            if (!IsRemoveMode)
            {
                IsRemoveMode = true;
                return;
            }

            if (removeProject.Count > 0)
            {
                var result = await OnShowMessageDialog($"{removeProject.Count} Peoject Remove.", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    removeProject.ForEach(p => ExecutePageViewModel.Remove(p));
                }
            }
            IsRemoveMode = false;
        }

        private void CancelExecute()
        {
            IsRemoveMode = false;
        }

        private void AddExecute()
        {
            var project = (IProject)ExecutePageViewModel.SelectedProject?.Clone();
            ExecutePageViewModel.Add(project);
        }

        private async void ExportExecuteAsync()
        {
            string fileName = "";
            var result = await OnShowCommonDialog(typeof(SaveFileDialog),
                (d) =>
                {
                    var sfd = ((SaveFileDialog)d);
                    sfd.Title = "Project Save";
                    sfd.Filter = loadModel.SaveFileter;
                },
                (d) =>
                {
                    var sfd = ((SaveFileDialog)d);
                    fileName = sfd.FileName;
                });

            if (result)
            {
                try
                {
                    loadModel.Save(ExecutePageViewModel.Context, fileName);
                }
                catch (Exception ex)
                {
                    _ = OnShowMessageDialog(ex.Message);
                }
            }
        }

        private async void ImportExecuteAsync()
        {
            string fileName = "";
            var result = await OnShowCommonDialog(typeof(OpenFileDialog),
                (d) =>
                {
                    var ofd = ((OpenFileDialog)d);
                    ofd.Title = "Project Open";
                    ofd.Filter = loadModel.OpenFileter;
                },
                (d) =>
                {
                    var ofd = ((OpenFileDialog)d);
                    fileName = ofd.FileName;
                });

            if (result)
            {
                try
                {
                    var Context = (IProjectContext<IProject>)loadModel.Load(fileName, ExecutePageViewModel.Context.GetType());
                    if (Context.Verion != ExecutePageViewModel.Context.Verion)
                    {
                        throw new Exception($"Version Error. Export Version:{Context.Verion} This Version:{ExecutePageViewModel.Context.Verion}");
                    }
                    else if (Context.Projects == null || Context.Projects.Count() == 0)
                    {
                        throw new Exception($"Export Error. Project is Nothing.");
                    }

                    foreach (var project in Context.Projects)
                    {
                        ExecutePageViewModel.Add(project);
                    }
                }
                catch (Exception ex)
                {
                    _ = OnShowMessageDialog(ex.Message);
                    return;
                }
            }
        }

        private void SelectedDefault(IProject project)
        {
            ExecutePageViewModel.SelectedProject = project;
        }

        private void SelectedRemove(IProject project)
        {
            if (removeProject.Contains(project))
            {
                removeProject.Remove(project);
            }
            else
            {
                removeProject.Add(project);
            }
        }

    }
}
