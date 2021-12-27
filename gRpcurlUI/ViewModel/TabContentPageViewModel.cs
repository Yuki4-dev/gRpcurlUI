﻿using gRpcurlUI.Model;
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
                    ExecutePageViewModel.SelectedProject = null;
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
