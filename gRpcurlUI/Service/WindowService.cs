using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Model;
using gRpcurlUI.View.Dialog;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace gRpcurlUI.Service
{
    public class WindowService : IWindowService
    {
        private Window? window;

        public Dispatcher? Dispatcher => window?.Dispatcher;

        public Brush AccentBrush
        {
            get
            {
                DwmGetColorizationColor(out var rgb, out var _);
                var color = Color.FromArgb((byte)(rgb >> 24), (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
                return new SolidColorBrush(color);
            }
        }

        public event Action<double, double>? WindowSizeChanged;

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

            if (window!.WindowState != WindowState.Maximized
                && !double.IsNaN(window.Height) && !double.IsNaN(window.Width))
            {
                WindowSizeChanged?.Invoke(window.Height, window.Width);
            }
        }

        public async Task<bool> ShowFileDialogAsync(FileDialogType dialogType, Action<IFileDialog> pre, Action<IFileDialog> post)
        {
            ThrowIfWindowNull();

            var dialog = GetDialog(dialogType);
            if (dialog is null)
            {
                throw new InvalidOperationException("dialog is null.");
            }

            return await Dispatcher!.InvokeAsync(() =>
            {
                var wrapper = new FileDialogWrapper(dialog);
                pre.Invoke(wrapper);
                var result = dialog.ShowDialog();
                post.Invoke(wrapper);
                return result ?? false;
            });
        }

        public async Task ShowDialogAsync(Type dialogType, object dataContext)
        {
            ThrowIfWindowNull();

            await Dispatcher!.InvokeAsync(() =>
            {
                var dialog = (Window?)Activator.CreateInstance(dialogType);
                if (dialog is null)
                {
                    throw new InvalidOperationException("dialog is null.");
                }

                dialog.DataContext = dataContext;
                _ = dialog.ShowDialog();
            });
        }

        public async Task<MessageBoxResult> ShowMessageDialogAsync(string title, string message, MessageBoxButton button = MessageBoxButton.OK)
        {
            ThrowIfWindowNull();

            return await Dispatcher!.InvokeAsync(() =>
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

        private static FileDialog GetDialog(FileDialogType dialogType)
        {
            if (dialogType == FileDialogType.Open)
            {
                return Activator.CreateInstance<OpenFileDialog>();
            }
            else if (dialogType == FileDialogType.Save)
            {
                return Activator.CreateInstance<SaveFileDialog>();
            }

            throw new InvalidOperationException($"Invalid Type {dialogType}");
        }

        [DllImport("Dwmapi.dll")]
        private static extern void DwmGetColorizationColor([Out] out int pcrColorization, [Out] out bool pfOpaqueBlend);

        private class FileDialogWrapper : IFileDialog
        {
            public string Title { get => fileDialog.Title; set => fileDialog.Title = value; }
            public string Filter { get => fileDialog.Filter; set => fileDialog.Filter = value; }
            public string FileName { get => fileDialog.FileName; set => fileDialog.FileName = value; }

            private readonly FileDialog fileDialog;

            public FileDialogWrapper(FileDialog fileDialog)
            {
                this.fileDialog = fileDialog;
            }
        }
    }
}
