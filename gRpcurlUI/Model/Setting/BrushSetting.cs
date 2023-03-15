using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;

namespace gRpcurlUI.Model.Setting
{
    public class BrushSetting : ResourceSetting
    {
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
            KEY_ScrollBarTabBrush
        };

        public Brush WindowBackground
        {
            get => (Brush)GetResources(KEY_WindowBackground);
            set => SetResources(KEY_WindowBackground, value);
        }

        public Brush PageBackground
        {
            get => (Brush)GetResources(KEY_PageBackground);
            set => SetResources(KEY_PageBackground, value);
        }

        public Brush PageForeground
        {
            get => (Brush)GetResources(KEY_PageForeground);
            set => SetResources(KEY_PageForeground, value);
        }

        public Brush BorderBrush
        {
            get => (Brush)GetResources(KEY_BorderBrush);
            set => SetResources(KEY_BorderBrush, value);
        }

        public Brush IconBrush
        {
            get => (Brush)GetResources(KEY_IconBrush);
            set => SetResources(KEY_IconBrush, value);
        }

        public Brush EditAreaTextBoxBrush
        {
            get => (Brush)GetResources(KEY_EditAreaTextBoxBrush);
            set => SetResources(KEY_EditAreaTextBoxBrush, value);
        }

        public Brush TextBoxSelectBrush
        {
            get => (Brush)GetResources(KEY_TextBoxSelectBrush);
            set => SetResources(KEY_TextBoxSelectBrush, value);
        }

        public Brush MouseOverBackground
        {
            get => (Brush)GetResources(KEY_MouseOverBackground);
            set => SetResources(KEY_MouseOverBackground, value);
        }

        public Brush SelectedBackground
        {
            get => (Brush)GetResources(KEY_SelectedBackground);
            set => SetResources(KEY_SelectedBackground, value);
        }

        public Brush ScrollBarTabBrush
        {
            get => (Brush)GetResources(KEY_ScrollBarTabBrush);
            set => SetResources(KEY_ScrollBarTabBrush, value);
        }

        public BrushSetting() : base() { }

        public BrushSetting(IDictionary resources) : base(resources) { }
    }
}
