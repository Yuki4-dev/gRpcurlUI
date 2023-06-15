using System.ComponentModel;

namespace gRpcurlUI.Core.Model
{
    public interface ISettingRow : INotifyPropertyChanged
    {
        public string Title { get; }

        public object Value { get; set; }

        public bool IsChanged { get; }

        public string DefaultValue { get; }
    }

    public interface ISettingValueConverter
    {
        public bool Convert(object value, out object newValue);
    }
}
