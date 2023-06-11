using gRpcurlUI.Core.Model;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace gRpcurlUI.Core.API
{
    public interface IWindowService
    {
        Dispatcher? Dispatcher { get; }

        Brush AccentBrush { get; }

        event Action<double, double>? WindowSizeChanged;

        void SetBaseWindow(Window window);

        Task ShowDialogAsync(Type dialogType, object dataContext);

        Task<MessageBoxResult> ShowMessageDialogAsync(string title, string message, MessageBoxButton button = MessageBoxButton.OK);

        Task<bool> ShowFileDialogAsync(FileDialogType dialogType, Action<IFileDialog> pre, Action<IFileDialog> post);
    }

    public enum FileDialogType
    {
        Open,
        Save,
    }
}
