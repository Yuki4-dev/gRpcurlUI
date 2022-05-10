using CommunityToolkit.Mvvm.DependencyInjection;
using gRpcurlUI.Core.Procces;
using gRpcurlUI.Service;
using gRpcurlUI.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            sc.AddSingleton<IWindowService, WindowService>();
            sc.AddSingleton<IProcessExecuter, ProcessExecuter>();
            sc.AddSingleton<IProjectDataService, ProjectDataService>();
            sc.AddSingleton<SettingPageViewModel>();

            Ioc.Default.ConfigureServices(sc.BuildServiceProvider());
        }
    }
}
