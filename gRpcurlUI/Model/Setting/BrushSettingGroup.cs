using gRpcurlUI.Core.Setting;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace gRpcurlUI.Model.Setting
{
    public class BrushSettingGroup : ResourceSettingProvider, ISettingGroup
    {
        public string Name => "Brush Setting";

        private readonly ObservableCollection<ISettingRow> settingRows = new();
        public ICollection<ISettingRow> SettingRows => settingRows;

        private const string KEY_WindowBackground = "DefaultWindowBackground";
        private const string KEY_PageBackground = "DefaultPageBackground";
        private const string KEY_PageForeground = "DefaultPageForeground";
        private const string KEY_BorderBrush = "DefaultBorderBrush";
        private const string KEY_IconBrush = "DefaultIconBrush";
        private const string KEY_EditAreaTextBoxBrush = "DefaultEditAreaTextBoxBrush";
        private const string KEY_TextBoxSelectBrush = "DefaultTextBoxSelectBrush";
        private const string KEY_MouseOverBackground = "DefaultMouseOverBackground";
        private const string KEY_SelectedBackground = "DefaultSelectedBackground";
        private const string KEY_ScrollBarTabBrush = "DefaultScrollBarTabBrush";
        private const string KEY_SettingExpanderAreaBackGround = "DefaultSettingExpanderAreaBackGround";
        private const string KEY_DefaultCationColor = "DefaultCationColor";

        protected override IEnumerable<string> Keys => new string[]
        {
            KEY_WindowBackground,
            KEY_PageBackground,
            KEY_PageForeground,
            KEY_BorderBrush,
            KEY_IconBrush,
            KEY_EditAreaTextBoxBrush,
            KEY_TextBoxSelectBrush,
            KEY_MouseOverBackground,
            KEY_SelectedBackground,
            KEY_ScrollBarTabBrush,
            KEY_SettingExpanderAreaBackGround,
            KEY_DefaultCationColor,
        };

        public BrushSettingGroup() : base() 
        {
            SetSettingRows();
        }

        public BrushSettingGroup(IDictionary resources) : base(resources) 
        {
            SetSettingRows();
        }

        private void SetSettingRows()
        {
            Keys.ToList().ForEach(k =>
            {
                settingRows.Add(new SettingRow(this, k, k, null, BrushConverter.Default));
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private class BrushConverter : ISettingValueConverter
        {
            public static readonly BrushConverter Default = new();

            public bool Convert(object value, out object newValue)
            {
                if (value is string str && TryColorParse(str, out var color))
                {
                    newValue = new SolidColorBrush(color);
                    return true;
                }

#pragma warning disable CS8625 // null リテラルを null 非許容参照型に変換できません。
                newValue = null;
#pragma warning restore CS8625 // null リテラルを null 非許容参照型に変換できません。
                return false;
            }

            private static bool TryColorParse(string text, out Color color)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    color = new Color();
                    return false;
                }
                try
                {
                    color = (Color)ColorConverter.ConvertFromString(text);
                    return true;
                }
                catch
                {
                    color = new Color();
                    return false;
                }
            }
        }
    }
}
