using CommunityToolkit.Mvvm.DependencyInjection;
using gRpcurlUI.Core.API;
using gRpcurlUI.Core.Converter.Proto.Analyze;
using gRpcurlUI.Core.Converter.Proto.Format;
using gRpcurlUI.Core.Process;
using gRpcurlUI.Core.Setting;
using gRpcurlUI.Model.Curl;
using gRpcurlUI.Model.Grpcurl;
using gRpcurlUI.Model.Setting;
using gRpcurlUI.Service;
using gRpcurlUI.View.Pages;
using gRpcurlUI.ViewModel;
using gRpcurlUI.ViewModel.Pages;
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

            // Core
            _ = sc.AddSingleton<ProtoAnalyzeEntry>();
            _ = sc.AddSingleton<ProtoFormatEntry>();
            _ = sc.AddSingleton<ProcessExecuterFactory>();

            // API
            _ = sc.AddSingleton<IWindowService, WindowService>();
            _ = sc.AddSingleton<IProjectDataService, ProjectDataService>();
            _ = sc.AddSingleton<IProjectContextProvider, ProjectContextProvider>();
            _ = sc.AddSingleton<IApplicationSetting, Service.ApplicationSetting>();

            // Pages
            _ = sc.AddSingleton<ProjectTabPage>();
            _ = sc.AddSingleton<SettingPage>();

            // ViewModels
            _ = sc.AddSingleton<MainWindowViewModel>();
            _ = sc.AddSingleton<SettingPageViewModel>();
            _ = sc.AddSingleton<ProjectTabPageViewModel>();

            // Settings
            _ = sc.AddSingleton<FontSettingGroup>();
            _ = sc.AddSingleton<BrushSettingGroup>();
            _ = sc.AddSingleton<GrpcurlSettingGroup>();

            // Projects
            _ = sc.AddSingleton<CurlProjectContext>();
            _ = sc.AddSingleton<GrpcurlProjectContext>();

            Ioc.Default.ConfigureServices(sc.BuildServiceProvider());
        }
    }
}
