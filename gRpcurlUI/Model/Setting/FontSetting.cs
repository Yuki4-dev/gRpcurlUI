using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace gRpcurlUI.Model.Setting
{
    public class FontSetting : ResourceSetting
    {
        private const string KEY_EditFontSize = "EditTextBoxFontSize";
        private const string KEY_FontFamily = "DefaultFontFamily";

        protected override IEnumerable<string> Keys => new string[]
        {
            KEY_EditFontSize,
            KEY_FontFamily
        };

        public string AppPath { get; set; }

        public double EditFontSize
        {
            get => (double)GetResources(KEY_EditFontSize);
            set => SetResources(KEY_EditFontSize, value);
        }

        public FontFamily FontFamily
        {
            get => (FontFamily)GetResources(KEY_FontFamily);
            set => SetResources(KEY_FontFamily, value);
        }

        public FontSetting() : base() { }

        public FontSetting(IDictionary resources) : base(resources) { }
    }
}
