using gRpcurlUI.Core.API;
using System;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DI.Injection();
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                ShowMessage(ex);
            }
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ShowMessage(e.Exception);
            e.Handled = true;
        }

        private void ShowMessage(Exception ex)
        {
            var window = DI.Get<IWindowService>();
            _ = window.ShowMessageDialogAsync("Error", ex.Message + Environment.NewLine + ex.StackTrace);
        }
    }
}
