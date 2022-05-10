using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace gRpcurlUI.Service
{
    public interface IWindowService
    {
        Dispatcher? Dispatcher { get; }

        event Action<double, double>? WindowSizeChenged;

        void SetBaseWindow(Window window);

        Task ShowDialogAsync(Type dialogType, object dataContext);

        Task<MessageBoxResult> ShowMessageDialogAsync(string title, string message, MessageBoxButton button = MessageBoxButton.OK);

        Task<bool> ShowCommonDialogAsync<T>(Action<T> pre, Action<T> post) where T : CommonDialog;
    }
}
