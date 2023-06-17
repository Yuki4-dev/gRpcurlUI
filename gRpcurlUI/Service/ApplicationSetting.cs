using gRpcurlUI.Core.Setting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace gRpcurlUI.Service
{
    public class ApplicationSetting : IApplicationSetting
    {
        public event Action<ApplicationSettingChangedEventArgs>? ApplicationSettingChanged;

        private readonly IDictionary<string, string> settingMap;

        public ApplicationSetting()
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

        public string? GetSettingValue(string key)
        {
            return settingMap.ContainsKey(key) ? settingMap[key] : null;
        }

        public void SetSettingValue(string key, string value)
        {
            if (!settingMap.ContainsKey(key) || !Equals(settingMap[key], value))
            {
                SetSettingValueInternal(key, value);
            }
        }

        private void SetSettingValueInternal(string key, string value)
        {
            settingMap[key] = value;
            GrpcurlSetting.Default.AppSetting = JsonConvert.SerializeObject(settingMap);
            GrpcurlSetting.Default.Save();
            ApplicationSettingChanged?.Invoke(new ApplicationSettingChangedEventArgs(key));
        }
    }
}
