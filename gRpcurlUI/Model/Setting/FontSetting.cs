using gRpcurlUI.Core.Model;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace gRpcurlUI.Model.Setting
{
    public class FontSetting : ResourceSetting, ISettingGroup
    {
        public string Name => nameof(FontSetting);

        private const string KEY_EditFontSize = "EditTextBoxFontSize";
        private const string KEY_FontFamily = "DefaultFontFamily";

        protected override IEnumerable<string> Keys => new string[]
        {
            KEY_EditFontSize,
            KEY_FontFamily
        };

        private readonly ObservableCollection<ISettingRow> settingRows = new();
        public ICollection<ISettingRow> SettingRows => settingRows;

        public FontSetting() : base()
        {
            SetSettingRows();
        }

        public FontSetting(IDictionary resources) : base(resources)
        {
            SetSettingRows();
        }

        private void SetSettingRows()
        {
            settingRows.Add(new ResourceSettingRow(this, KEY_EditFontSize, KEY_EditFontSize));
            settingRows.Add(new ResourceSettingRow(this, KEY_FontFamily, KEY_FontFamily, new FontFamilyConverter()));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private class FontFamilyConverter : ISettingValueConverter
        {
            public bool Convert(object value, out object newValue)
            {
                if (value is string font && !string.IsNullOrWhiteSpace(font))
                {
                    var ff = new FontFamily(font);
                    if (ff.FamilyNames.Values.Contains(font))
                    {
                        newValue = ff;
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
