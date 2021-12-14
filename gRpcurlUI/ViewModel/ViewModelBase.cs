using gRpcurlUI.Model;
using Microsoft.Win32;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace gRpcurlUI.ViewModel
{
    public class ViewModelBase : Observable
    {
        public event Func<string, MessageBoxButton, Task<MessageBoxResult>> ShowMessageDialog;

        public event Func<Type, object, Task> ShowDialog;

        public event Func<Type, Action<CommonDialog>, Action<CommonDialog>, Task<bool>> ShowCommonDialog;

        protected Task<MessageBoxResult> OnShowMessageDialog(string message, MessageBoxButton button = MessageBoxButton.OK)
        {
            return ShowMessageDialog?.Invoke(message, button) ?? Task.FromResult(MessageBoxResult.None);
        }

        protected Task OnShowDialog(Type dialogType, object dataContext)
        {
            return ShowDialog?.Invoke(dialogType, dataContext) ?? Task.CompletedTask;
        }

        protected Task<bool> OnShowCommonDialog(Type dialogType, Action<CommonDialog> preCallback, Action<CommonDialog> succesCallback)
        {
            return ShowCommonDialog?.Invoke(dialogType, preCallback, succesCallback) ?? Task.FromResult(false);
        }
    }
}
