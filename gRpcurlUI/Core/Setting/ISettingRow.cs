using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace gRpcurlUI.Core.Setting
{
    public interface ISettingRow : INotifyPropertyChanged
    {
        public string Title { get; }

        public string Description { get; }

        public SettingRowInputType InputType { get; }

        public object? Value { get; set; }

        public bool IsChanged { get; }

        public string DefaultValue { get; }
    }

    public enum SettingRowInputType
    {
        None, Text, Switch, DropDown
    }

    public partial class SettingRow : ObservableObject, IDropDownSettingRow
    {
        [ObservableProperty]
        private string title;

        [ObservableProperty]
        private string description = string.Empty;

        [ObservableProperty]
        private bool isChanged = false;

        [ObservableProperty]
        private SettingRowInputType inputType = SettingRowInputType.Text;

        [ObservableProperty]
        private string defaultValue = string.Empty;

        [ObservableProperty]
        private ICollection<object> items = new ObservableCollection<object>();

        public object? Value
        {
            get => settingValueProvider.GetSetting(key);
            set
            {
                if (settingValueProvider.GetSetting(key) != value)
                {
                    if (settingValueConverter != null)
                    {
                       if(settingValueConverter.Convert(value, out var newValue))
                        {
                            settingValueProvider.SetSetting(key, newValue);
                        }
                        OnPropertyChanged();
                    }
                    else
                    {
                        settingValueProvider.SetSetting(key, value);
                        OnPropertyChanged();
                    }
                }
            }
        }


        private readonly ISettingValueProvider settingValueProvider;

        private readonly string key;

        private readonly ISettingValueConverter? settingValueConverter;

        public SettingRow(ISettingValueProvider settingValueProvider, string title, string key, string? defaultValue = null) : this(settingValueProvider, title, key, null, null, defaultValue)
        {
        }

        public SettingRow(ISettingValueProvider settingValueProvider, string title, string key, string? description, ISettingValueConverter? settingValueConverter, string? defaultValue = null)
        {
            this.settingValueProvider = settingValueProvider;
            this.title = title;
            this.key = key;
            this.settingValueConverter = settingValueConverter;

            if (description != null)
            {
                Description = description;
            }

            if (defaultValue != null)
            {
                DefaultValue = defaultValue;
            }
        }
    }
}
