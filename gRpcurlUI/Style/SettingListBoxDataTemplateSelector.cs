using gRpcurlUI.Core.Setting;
using System;
using System.Windows;
using System.Windows.Controls;

namespace gRpcurlUI.Style
{
    public class SettingListBoxDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NoneDataTemplate { get; set; } = new DataTemplate();

        public DataTemplate TextInputDataTemplate { get; set; }

        public DataTemplate SwitchInputDataTemplate { get; set; }

        public DataTemplate DropDownInputDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ISettingRow settingRow)
            {
                if (settingRow.InputType == SettingRowInputType.Text)
                {
                    return TextInputDataTemplate;
                }
                else if(settingRow.InputType == SettingRowInputType.Switch)
                {
                    return SwitchInputDataTemplate;
                }
                else if (settingRow.InputType == SettingRowInputType.DropDown)
                {
                    return DropDownInputDataTemplate;
                }
                else
                {
                    return NoneDataTemplate;
                }
            }

            throw new Exception($"{nameof(SettingListBoxDataTemplateSelector)} Not Support {item.GetType()}");
        }
    }
}
