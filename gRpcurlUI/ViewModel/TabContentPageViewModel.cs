using gRpcurlUI.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace gRpcurlUI.ViewModel
{
    public class TabContentPageViewModel : ViewModelBase
    {
        private ExecutePageViewModel _ExcutePageViewModel;
        public ExecutePageViewModel ExecutePageViewModel
        {
            get => _ExcutePageViewModel;
            set => OnPropertyChanged(ref _ExcutePageViewModel, value);
        }

        private IProjectContext _ProjectContext;
        public IProjectContext ProjectContext
        {
            get => _ProjectContext;
            set
            {
                if (SetProperty(ref _ProjectContext, value))
                {
                    SelectedDefault(null);
                    OnPropertyChanged();
                }
            }
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

        public ICommand SelectedCommand { get; }

        public ICommand AddCommand { get; }

        public ICommand RemoveCommand { get; }

        public ICommand ImportCommand { get; }

        public ICommand ExportCommand { get; }

        public ICommand CancelCommand { get; }

        private readonly ILoadModel loadModel;

        private readonly List<IProject> removeProject = new List<IProject>();

        public TabContentPageViewModel() : this(new JsonLoadModel(), new ExecutePageViewModel()) { }

        public TabContentPageViewModel(ILoadModel load, ExecutePageViewModel executePageViewmodel)
        {
            loadModel = load;
            ExecutePageViewModel = executePageViewmodel;

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
                    removeProject.ForEach(p => ProjectContext.RemoveProject(p));
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
            ProjectContext.AddProject();
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
                    loadModel.Save(ProjectContext, fileName);
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
                    var Context = (IProjectContext)loadModel.Load(fileName, ProjectContext.GetType());
                    ProjectContext.Marge(Context);
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
