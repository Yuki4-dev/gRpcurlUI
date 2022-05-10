using gRpcurlUI.View.Dialog;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace gRpcurlUI.Service
{
    public class WindowService : IWindowService
    {
        private Window? window;

        public Dispatcher? Dispatcher => window?.Dispatcher;

        public event Action<double, double>? WindowSizeChenged;

        public void SetBaseWindow(Window window)
        {
            if (this.window != null)
            {
                this.window.SizeChanged -= Window_SizeChanged;
            }

            this.window = window;
            this.window.SizeChanged += Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ThrowIfWindowNull();

            if (window.WindowState != WindowState.Maximized
                && window.Height != double.NaN
                && window.Width != double.NaN)
            {
                WindowSizeChenged?.Invoke(window.Height, window.Width);
            }
        }

        public async Task<bool> ShowCommonDialogAsync<T>(Action<T> pre, Action<T> post) where T : CommonDialog
        {
            ThrowIfWindowNull();

            return await Dispatcher.InvokeAsync(() =>
            {
                var dialog = (T?)Activator.CreateInstance(typeof(T));
                if (dialog is null)
                {
                    throw new InvalidOperationException("dialog is null.");
                }

                pre.Invoke(dialog);
                var result = dialog.ShowDialog();
                post.Invoke(dialog);
                return result ?? false;
            });
        }

        public async Task ShowDialogAsync(Type dialogType, object dataContext)
        {
            ThrowIfWindowNull();

            await Dispatcher.InvokeAsync(() =>
            {
                var dialog = (Window?)Activator.CreateInstance(dialogType);
                if(dialog is null)
                {
                    throw new InvalidOperationException("dialog is null.");
                }

                dialog.DataContext = dataContext;
                dialog.ShowDialog();
            });
        }

        public async Task<MessageBoxResult> ShowMessageDialogAsync(string title, string message, MessageBoxButton button = MessageBoxButton.OK)
        {
            ThrowIfWindowNull();

            return await Dispatcher.InvokeAsync(() =>
            {
                return MessageDialog.Show(title, message, button);
            });
        }

        private void ThrowIfWindowNull()
        {
            if (window is null)
            {
                throw new InvalidOperationException("window is null.");
            }
        }
    }
}
