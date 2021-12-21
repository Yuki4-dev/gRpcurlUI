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

        void AddViewModel(ViewModelBase viewModel);

        void RemoveViewModel(ViewModelBase viewModel);
    }

    public class WindowOwner : IWindowOwner
    {
        private readonly List<ViewModelBase> viewModels = new List<ViewModelBase>();

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

        public void AddViewModel(ViewModelBase viewModel)
        {
            viewModels.Add(viewModel);
            viewModel.ShowMessageDialog += ShowMessageDialogAsync;
            viewModel.ShowCommonDialog += ShowCommonDialogAsync;
            viewModel.ShowDialog += ShowDialogAsync;
        }

        public void RemoveViewModel(ViewModelBase viewModel)
        {
            if (viewModels.Remove(viewModel))
            {
                viewModel.ShowMessageDialog -= ShowMessageDialogAsync;
                viewModel.ShowCommonDialog -= ShowCommonDialogAsync;
                viewModel.ShowDialog -= ShowDialogAsync;
            }
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
