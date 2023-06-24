using gRpcurlUI.Core.Setting;
using System;
using System.Collections;
using System.Windows;

namespace gRpcurlUI.Model.Setting
{
    public class ResourceSettingProvider : ISettingValueProvider
    {
        private readonly IDictionary resources;

        public ResourceSettingProvider()
        {
            resources = App.Current.Resources;
        }

        public ResourceSettingProvider(IDictionary resources)
        {
            this.resources = resources;
        }

        public object? GetSetting(string key)
        {
            return resources[key];
        }

        public void SetSetting(string key, object? value)
        {
            if (resources.Contains(key) && resources[key] == value)
            {
                return;
            }

            if (resources is ResourceDictionary rdic)
            {
                rdic.Remove(key);
                rdic.Add(key, value);
            }
            else
            {
                resources[key] = value;
            }

            SettingChanged?.Invoke(new SettingChangedEventArgs(key));
        }

        public event Action<SettingChangedEventArgs>? SettingChanged;
    }
}
