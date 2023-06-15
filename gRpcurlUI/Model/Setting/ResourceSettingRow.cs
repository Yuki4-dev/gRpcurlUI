using gRpcurlUI.Core.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace gRpcurlUI.Model.Setting
{
    public class ResourceSettingRow : ISettingRow
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public object Value
        {
            get => resourceSetting.GetResources(key);
            set
            {
                if (resourceSetting.GetResources(key) != value)
                {
                    if (settingValueConverter != null && settingValueConverter.Convert(value, out var newValue))
                    {
                        resourceSetting.SetResources(key, newValue);
                        OnPropertyChanged();
                    }
                    else
                    {
                        resourceSetting.SetResources(key, value);
                        OnPropertyChanged();
                    }
                }
            }
        }

        private bool _isChanged = false;
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DefaultValue { get; set; } = string.Empty;

        private readonly ResourceSetting resourceSetting;

        private readonly string key;

        private readonly ISettingValueConverter? settingValueConverter;

        public ResourceSettingRow(ResourceSetting resourceSetting, string title, string key, string? defaultValue = null) : this(resourceSetting, title, key, null, defaultValue)
        {
        }

        public ResourceSettingRow(ResourceSetting resourceSetting, string title, string key, ISettingValueConverter? settingValueConverter, string? defaultValue = null)
        {
            this.resourceSetting = resourceSetting;
            _title = title;
            this.key = key;
            this.settingValueConverter = settingValueConverter;

            if (defaultValue != null)
            {
                DefaultValue = defaultValue;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
