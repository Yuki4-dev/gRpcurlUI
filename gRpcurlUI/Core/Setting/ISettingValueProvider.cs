using System;

namespace gRpcurlUI.Core.Setting
{
    public interface ISettingValueProvider
    {
        event Action<SettingChangedEventArgs>? SettingChanged;

        public object? GetSetting(string key);

        public void SetSetting(string key, object value);
    }

    public class SettingChangedEventArgs
    {
        public string Key { get; }
        public SettingChangedEventArgs(string key)
        {
            Key = key;
        }
    }
}
