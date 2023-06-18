using gRpcurlUI.Core.Setting;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace gRpcurlUI.Model.Setting
{
    public class FontSettingGroup : ResourceSettingProvider, ISettingGroup
    {
        public string Name => Language.Default.SettingPage.FontSettingTitle;

        private const string KEY_EditFontSize = "EditTextBoxFontSize";
        private const string KEY_FontFamily = "DefaultFontFamily";

        protected override IEnumerable<string> Keys => new string[]
        {
            KEY_EditFontSize,
            KEY_FontFamily
        };

        private readonly ObservableCollection<ISettingRow> settingRows = new();
        public ICollection<ISettingRow> SettingRows => settingRows;

        public FontSettingGroup() : base()
        {
            SetSettingRows();
        }

        public FontSettingGroup(IDictionary resources) : base(resources)
        {
            SetSettingRows();
        }

        private void SetSettingRows()
        {
            var fontSize = new SettingRow(this, Language.Default.SettingPage.FontSize, KEY_EditFontSize, null, new FontSizeConverter());
            settingRows.Add(fontSize);

            //var fontfam = new SettingRow(this, KEY_FontFamily, KEY_FontFamily, null);
            //fontfam.Value = GetSetting(KEY_FontFamily);
            //fontfam.InputType = SettingRowInputType.DropDown;
            //var  ifc = new InstalledFontCollection();
            //fontfam.Items = new List<object>(ifc.Families.Select(f => f.Name));
            //settingRows.Add(fontfam);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private class FontSizeConverter : ISettingValueConverter
        {
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
