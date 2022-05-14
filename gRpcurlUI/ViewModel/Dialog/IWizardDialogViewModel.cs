using System;

namespace gRpcurlUI.ViewModel.Dialog
{
    public interface IWizardDialogViewModel
    {
        public Type PageType { get; }

        public bool CanNext();

        public bool CanBack();

        public bool CanSuccess();

        public void Navigate();

        public void Success();

        public void Close();
    }
}
