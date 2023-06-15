using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using gRpcurlUI.Core.API;
using gRpcurlUI.View.Dialog.Proto;
using System;
using System.IO;
using System.Threading.Tasks;

namespace gRpcurlUI.ViewModel.Dialog.Proto
{
    [ObservableObject]
    public partial class ProtoImportPage1ViewModel : IWizardDialogViewModel
    {
        public Type PageType => typeof(ProtoImportPage1);

        public string FilePath
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
            if (string.IsNullOrEmpty(FilePath))
            {
                ErrorMessage = "FilePath is Empty.";
                return false;
            }

            if (!File.Exists(FilePath))
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

        [RelayCommand]
        private async Task OpenFile()
        {
            string fileName = string.Empty;
            var result = await windowService.ShowFileDialogAsync(
                FileDialogType.Open,
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
                FilePath = fileName;
            }
        }
    }
}
