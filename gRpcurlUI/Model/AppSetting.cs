using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace gRpcurlUI.Model
{
    public interface IReadOnlyAppSetting
    {
        string AppPath { get; }

        double EditFontSize { get; }

        FontFamily FontFamily { get; }

        BrushSetting BrushSetting { get; }
    }

    public interface IReadOnlyBrushSetting
    {
        Brush WindowBackground { get; }

        Brush PageBackground { get; }

        Brush PageForeground { get; }

        Brush BorderBrush { get; }

        Brush IconBrush { get; }

        Brush EditAreaBoxBrush { get; }

        Brush TextBoxSelectBrush { get; }

        Brush MouseOverBackground { get; }

        Brush SelectedBackground { get; }

        Brush ScrolBarTabBrush { get; }
    }

    public abstract class ResourceSetting
    {
        private readonly IDictionary resources;

        private IDictionary buffer;

        public ResourceSetting()
        {
            resources = new Dictionary<string, object>();
        }

        public ResourceSetting(IDictionary resources)
        {
            this.resources = resources;
        }

        public void Caputure()
        {
            buffer = CopyResources();
        }

        public void ResetToCaputure()
        {
            if (buffer == null)
            {
                throw new InvalidOperationException();
            }

            InsertResources(buffer);
        }

        public IDictionary CopyResources()
        {
            var newResources = new Dictionary<string, object>();
            foreach (var key in Keys)
            {
                newResources.Add(key, resources[key]);
            }
            return newResources;
        }

        public void InsertResources(IDictionary otherResources)
        {
            foreach (var key in Keys)
            {
                SetResources(key, otherResources[key]);
            }
        }

        public object GetResources(string key)
        {
            return resources[key];
        }

        public void SetResources(string key, object value)
        {
            if (resources is ResourceDictionary rdic)
            {
                rdic.Remove(key);
                rdic.Add(key, value);
            }
            else
            {
                resources[key] = value;
            }
        }

        protected abstract IEnumerable<string> Keys { get; }
    }

    public class BrushSetting : ResourceSetting, IReadOnlyBrushSetting
    {
        private const string KEY_WindowBackground = "DefaultWindowBackground";
        private const string KEY_PageBackground = "DefaultPageBackground";
        private const string KEY_PageForeground = "DefaultPageForeground";
        private const string KEY_BorderBrush = "DefaulBorderBrush";
        private const string KEY_IconBrush = "DefaulIconBrush";
        private const string KEY_EditAreaBoxBrush = "DefaulEditAreaBoxBrush";
        private const string KEY_TextBoxSelectBrush = "DefaultTextBoxSelectBrush";
        private const string KEY_MouseOverBackground = "DefaultMouseOverBackground";
        private const string KEY_SelectedBackground = "DefaultSelectedBackground";
        private const string KEY_ScrolBarTabBrush = "DefaulScrolBarTabBrush";

        protected override IEnumerable<string> Keys => new string[]
        {
            KEY_WindowBackground,
            KEY_PageBackground,
            KEY_PageForeground,
            KEY_BorderBrush,
            KEY_IconBrush,
            KEY_EditAreaBoxBrush,
            KEY_TextBoxSelectBrush,
            KEY_MouseOverBackground,
            KEY_SelectedBackground,
            KEY_ScrolBarTabBrush
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

        public Brush EditAreaBoxBrush
        {
            get => (Brush)GetResources(KEY_EditAreaBoxBrush);
            set => SetResources(KEY_EditAreaBoxBrush, value);
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

        public Brush ScrolBarTabBrush
        {
            get => (Brush)GetResources(KEY_ScrolBarTabBrush);
            set => SetResources(KEY_ScrolBarTabBrush, value);
        }

        public BrushSetting() : base() { }

        public BrushSetting(IDictionary resources) : base(resources) { }
    }

    public class AppSetting : ResourceSetting, IReadOnlyAppSetting
    {
        private const string KEY_EditFontSize = "EditTextBoxFontSize";
        private const string KEY_FontFamily = "DefaultFontFamily";

        protected override IEnumerable<string> Keys => new string[]
        {
            KEY_EditFontSize,
            KEY_FontFamily
        };

        public string AppPath { get; set; }

        public BrushSetting BrushSetting { get; set; }

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

        public AppSetting() : base()
        {
            BrushSetting = new BrushSetting();
        }

        public AppSetting(IDictionary resources) : base(resources)
        {
            BrushSetting = new BrushSetting(resources);
        }

    }
}
