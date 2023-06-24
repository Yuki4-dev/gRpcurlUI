using gRpcurlUI.Core.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace gRpcurlUI.Service
{
    public class ApplicationSettingProvider : ISettingValueProvider
    {
        private readonly IDictionary<string, string> settingMap;

        public ApplicationSettingProvider()
        {
            var appSettingJson = GrpcurlSetting.Default.AppSetting;
            if (string.IsNullOrWhiteSpace(appSettingJson))
            {
                settingMap = new Dictionary<string, string>();
                GrpcurlSetting.Default.AppSetting = JsonConvert.SerializeObject(settingMap);
                GrpcurlSetting.Default.Save();
            }
            else
            {
                settingMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(appSettingJson)!;
            }
        }

        public object? GetSetting(string key)
        {
            return settingMap.ContainsKey(key) ? settingMap[key] : null;
        }

        public void SetSetting(string key, object value)
        {
            if (value is not string)
            {
                throw new Exception($"value is not string. {key}:{value.GetType()}");
            }

            if (!settingMap.ContainsKey(key) || !Equals(settingMap[key], value))
            {
                SetSettingValueInternal(key, value);
            }
        }

        private void SetSettingValueInternal(string key, object value)
        {
            settingMap[key] = (string)value;
            GrpcurlSetting.Default.AppSetting = JsonConvert.SerializeObject(settingMap);
            GrpcurlSetting.Default.Save();
            SettingChanged?.Invoke(new SettingChangedEventArgs(key));
        }

        public event Action<SettingChangedEventArgs>? SettingChanged;
    }
}
