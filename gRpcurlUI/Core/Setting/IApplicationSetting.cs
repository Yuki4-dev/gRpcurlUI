using System;

namespace gRpcurlUI.Core.Setting
{
    public interface IApplicationSetting
    {
        event Action<ApplicationSettingChangedEventArgs> ApplicationSettingChanged;

        public string? GetSettingValue(string key);

        public void SetSettingValue(string key, string value);
    }

    public class ApplicationSettingChangedEventArgs
    {
        public string Key { get; }
        public ApplicationSettingChangedEventArgs(string key)
        {
            Key = key;
        }
    }
}
