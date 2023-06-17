namespace gRpcurlUI.Core.Setting
{
    public interface ISettingValueConverter
    {
        public bool Convert(object value, out object newValue);
    }
}
