﻿using CommunityToolkit.Mvvm.DependencyInjection;
using gRpcurlUI.Core.Converter.Proto.Analyze;
using gRpcurlUI.Core.Converter.Proto.Format;
using gRpcurlUI.Core.Procces;
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
            sc.AddSingleton<ProtoAnalyzeEntry>();
            sc.AddSingleton<ProtoFormatEntry>();
            sc.AddSingleton<IWindowService, WindowService>();
            sc.AddSingleton<IProcessExecuter, ProcessExecuter>();
            sc.AddSingleton<IProjectDataService, ProjectDataService>();
            sc.AddSingleton<SettingPageViewModel>();
            sc.AddSingleton<MainWindowViewModel>();

            Ioc.Default.ConfigureServices(sc.BuildServiceProvider());
        }
    }
}
