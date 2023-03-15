using CommunityToolkit.Mvvm.DependencyInjection;
using gRpcurlUI.Core.Converter.Proto.Analyze;
using gRpcurlUI.Core.Converter.Proto.Format;
using gRpcurlUI.Service;
using gRpcurlUI.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace gRpcurlUI
{
    public class DI
    {
        public static T Get<T>() where T : class
        {
            var services = Ioc.Default.GetService<T>() ?? throw new InvalidOperationException($"UnRegister : {typeof(T)}.");
            return services;
        }

        public static void Injection()
        {
            var sc = new ServiceCollection();
            _ = sc.AddSingleton<ProtoAnalyzeEntry>();
            _ = sc.AddSingleton<ProtoFormatEntry>();
            _ = sc.AddSingleton<IWindowService, WindowService>();
            _ = sc.AddSingleton<IProjectDataService, ProjectDataService>();
            _ = sc.AddSingleton<SettingPageViewModel>();
            _ = sc.AddSingleton<MainWindowViewModel>();

            Ioc.Default.ConfigureServices(sc.BuildServiceProvider());
        }
    }
}
