using gRpcurlUI.View.Dialog;
using gRpcurlUI.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace gRpcurlUI.View
{
    public interface IWindowOwner
    {
        Dispatcher Dispatcher { get; }

        event Action<double, double> WindowSizeChenged;

        void SetViewModel(ViewModelBase viewModel);
    }

    public class WindowOwner : IWindowOwner
    {
        private readonly Dictionary<Type, ViewModelBase> viewModels = new Dictionary<Type, ViewModelBase>();

        public event Action<double, double> WindowSizeChenged;

        public Dispatcher Dispatcher => window?.Dispatcher;

        private readonly Window window;

        public WindowOwner(Window window)
        {
            this.window = window;
            this.window.SizeChanged += Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (window.WindowState != WindowState.Maximized 
                && window.Height != double.NaN 
                && window.Width != double.NaN)
            {
                WindowSizeChenged?.Invoke(window.Height, window.Width);
            }
        }

        public void SetViewModel(ViewModelBase vm)
        {
            var key = vm.GetType();
            if (viewModels.ContainsKey(key))
            {
                var oldVm = viewModels[key];
                oldVm.ShowMessageDialog -= ShowMessageDialogAsync;
                oldVm.ShowCommonDialog -= ShowCommonDialogAsync;
                vm.ShowDialog -= ShowDialogAsync;

                viewModels.Remove(key);
            }

            viewModels.Add(key, vm);
            vm.ShowMessageDialog += ShowMessageDialogAsync;
            vm.ShowCommonDialog += ShowCommonDialogAsync;
            vm.ShowDialog += ShowDialogAsync;
        }

        private async Task ShowDialogAsync(Type dialogType, object dataContext)
        {
            await Dispatcher.InvokeAsync(() =>
            {
                var dialog = (Window)Activator.CreateInstance(dialogType);
                dialog.DataContext = dataContext;
                dialog.ShowDialog();
            });
        }

        private async Task<MessageBoxResult> ShowMessageDialogAsync(string message, MessageBoxButton button)
        {
            return await Dispatcher.InvokeAsync(() =>
            {
                return MessageDialog.Show(window.Title, message, button);
            });
        }

        private async Task<bool> ShowCommonDialogAsync(Type dialogType, Action<CommonDialog> pre, Action<CommonDialog> post)
        {
            return await Dispatcher.InvokeAsync(() =>
            {
                var dialog = (CommonDialog)Activator.CreateInstance(dialogType);
                pre.Invoke(dialog);
                var result = dialog.ShowDialog();
                post.Invoke(dialog);
                return result ?? false;
            });
        }
    }
}
