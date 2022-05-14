using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Service;
using gRpcurlUI.View.Proto;
using gRpcurlUI.ViewModel.Dialog;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;

namespace gRpcurlUI.ViewModel.Proto
{
    [ObservableObject]
    public partial class ProtoImportPage1ViewModel : IWizardDialogViewModel
    {
        public Type PageType => typeof(ProtoImportPage1);

        public string Filepath
        {
            get => protoImportPageShareSetting.FilePath;
            set
            {
                protoImportPageShareSetting.FilePath = value;
                OnPropertyChanged();
            }
        }

        [ObservableProperty]
        private string errorMessage = string.Empty;

        private readonly IWindowService windowService;

        private readonly ProtoImportPageShareSetting protoImportPageShareSetting;

        public ProtoImportPage1ViewModel(ProtoImportPageShareSetting protoImportPageShareSetting, IWindowService windowService)
        {
            this.windowService = windowService;
            this.protoImportPageShareSetting = protoImportPageShareSetting;
        }

        public bool CanBack()
        {
            return false;
        }

        public bool CanNext()
        {
            if (string.IsNullOrEmpty(Filepath))
            {
                ErrorMessage = "FilePath is Empty.";
                return false;
            }

            if (!File.Exists(Filepath))
            {
                ErrorMessage = "File Not Exists.";
                return false;
            }

            ErrorMessage = string.Empty;
            return true;
        }

        public bool CanSuccess()
        {
            return false;
        }

        public void Success()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            //
        }

        public void Navigate()
        {
            ErrorMessage = string.Empty;
        }

        [ICommand]
        private async Task OpenFile()
        {
            string fileName = string.Empty;
            var result = await windowService.ShowCommonDialogAsync<OpenFileDialog>(
                (d) =>
                {
                    d.Title = "Project Open";
                    d.Filter = "proto(*.proto)|*.proto";
                },
                (d) =>
                {
                    fileName = d.FileName;
                });

            if (result)
            {
                Filepath = fileName;
            }
        }
    }
}
