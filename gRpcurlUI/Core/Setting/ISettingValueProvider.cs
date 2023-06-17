namespace gRpcurlUI.Core.Setting
{
    public interface ISettingValueProvider
    {
        public object? GetSetting(string key);

        public void SetSetting(string key, object value);
    }

    public class ApplicationSettingValueProvider : ISettingValueProvider
    {
        private readonly IApplicationSetting applicationSettingProvider;

        public ApplicationSettingValueProvider(IApplicationSetting applicationSetting)
        {
            applicationSettingProvider = applicationSetting;
        }

        public object? GetSetting(string key)
        {
            return applicationSettingProvider.GetSettingValue(key);
        }

        public void SetSetting(string key, object value)
        {
            applicationSettingProvider.SetSettingValue(key, value.ToString()!);
        }
    }
}
