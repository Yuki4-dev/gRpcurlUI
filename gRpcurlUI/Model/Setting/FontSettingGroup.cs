using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Setting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.Model.Setting
{
    public class FontSettingGroup : ObservableObject, ISettingGroup
    {
        public string Name => Language.Default.SettingPage.FontSettingTitle;

        private const string KEY_EditFontSize = "EditTextBoxFontSize";
        private const string KEY_FontFamily = "DefaultFontFamily";

        private readonly ObservableCollection<ISettingRow> settingRows = new();
        public ICollection<ISettingRow> SettingRows => settingRows;

        private readonly ResourceSettingProvider resourceSettingProvider;

        public FontSettingGroup(ResourceSettingProvider resourceSettingProvider)
        {
            this.resourceSettingProvider = resourceSettingProvider;
            SetSettingRows();
        }

        private void SetSettingRows()
        {
            var fontSize = new SettingRow(resourceSettingProvider, Language.Default.SettingPage.FontSize, KEY_EditFontSize, null, FontSizeConverter.Default);
            settingRows.Add(fontSize);

            //var fontfam = new SettingRow(this, KEY_FontFamily, KEY_FontFamily, null);
            //fontfam.Value = GetSetting(KEY_FontFamily);
            //fontfam.InputType = SettingRowInputType.DropDown;
            //var  ifc = new InstalledFontCollection();
            //fontfam.Items = new List<object>(ifc.Families.Select(f => f.Name));
            //settingRows.Add(fontfam);
        }

        private class FontSizeConverter : ISettingValueConverter
        {
            public static readonly FontSizeConverter Default = new();

            public bool Convert(object value, out object newValue)
            {
                if (value is string size && !string.IsNullOrWhiteSpace(size))
                {
                    if (double.TryParse(size, out var dSize))
                    {
                        newValue = dSize;
                        return true;
                    }

                }

#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                newValue = null;
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                return false;
            }
        }
    }
}
