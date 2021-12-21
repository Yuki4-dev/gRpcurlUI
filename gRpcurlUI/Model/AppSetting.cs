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

    public class AppSetting : IReadOnlyAppSetting
    {
        private const string KEY_EditFontSize = "EditTextBoxFontSize";
        private const string KEY_FontFamily = "DefaultFontFamily";

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

        private readonly IDictionary resources = new Dictionary<string, object>();

        private string _AppPath;
        public string AppPath
        {
            get => _AppPath;
            set => _AppPath = value;
        }

        public double EditFontSize
        {
            get => (double)resources[KEY_EditFontSize];
            set
            {
                SetResources(KEY_EditFontSize, value);
            }
        }

        public FontFamily FontFamily
        {
            get => (FontFamily)resources[KEY_FontFamily];
            set
            {
                resources[KEY_FontFamily] = value;
            }
        }

        public Brush WindowBackground
        {
            get => (Brush)resources[KEY_WindowBackground];
            set
            {
                SetResources(KEY_WindowBackground, value);
            }
        }

        public Brush PageBackground
        {
            get => (Brush)resources[KEY_PageBackground];
            set
            {
                SetResources(KEY_PageBackground, value);
            }
        }

        public Brush PageForeground
        {
            get => (Brush)resources[KEY_PageForeground];
            set
            {
                SetResources(KEY_PageForeground, value);
            }
        }

        public Brush BorderBrush
        {
            get => (Brush)resources[KEY_BorderBrush];
            set
            {
                SetResources(KEY_BorderBrush, value);
            }
        }

        public Brush IconBrush
        {
            get => (Brush)resources[KEY_IconBrush];
            set
            {
                SetResources(KEY_IconBrush, value);
            }
        }

        public Brush EditAreaBoxBrush
        {
            get => (Brush)resources[KEY_EditAreaBoxBrush];
            set
            {
                SetResources(KEY_EditAreaBoxBrush, value);
            }
        }

        public Brush TextBoxSelectBrush
        {
            get => (Brush)resources[KEY_TextBoxSelectBrush];
            set
            {
                SetResources(KEY_TextBoxSelectBrush, value);
            }
        }

        public Brush MouseOverBackground
        {
            get => (Brush)resources[KEY_MouseOverBackground];
            set
            {
                SetResources(KEY_MouseOverBackground, value);
            }
        }

        public Brush SelectedBackground
        {
            get => (Brush)resources[KEY_SelectedBackground];
            set
            {
                SetResources(KEY_SelectedBackground, value);
            }
        }

        public Brush ScrolBarTabBrush
        {
            get => (Brush)resources[KEY_ScrolBarTabBrush];
            set
            {
                SetResources(KEY_ScrolBarTabBrush, value);
            }
        }

        private AppSetting startUpSetting;

        private AppSetting caputureSetting;

        public AppSetting() { }

        public AppSetting(IDictionary resources)
        {
            this.resources = resources;
            startUpSetting = new AppSetting();
            Copy(this, startUpSetting);
        }

        public void OnCaputure()
        {
            if(caputureSetting == null)
            {
                caputureSetting = new AppSetting();
            }
            Copy(this, caputureSetting);
        }

        public void Reset()
        {
            if (caputureSetting == null)
            {
                throw new InvalidOperationException();
            }
            Copy(caputureSetting, this);
        }

        public void ResetStartUp()
        {
            if (startUpSetting == null)
            {
                throw new InvalidOperationException();
            }
            Copy(startUpSetting, this);
        }

        private void Copy(AppSetting baseSetting, AppSetting caputure)
        {
            caputure.AppPath = baseSetting.AppPath;
            caputure.EditFontSize = baseSetting.EditFontSize;
            caputure.FontFamily = baseSetting.FontFamily;
            caputure.WindowBackground = baseSetting.WindowBackground;
            caputure.PageBackground = baseSetting.PageBackground;
            caputure.PageForeground = baseSetting.PageForeground;
            caputure.BorderBrush = baseSetting.BorderBrush;
            caputure.IconBrush = baseSetting.IconBrush;
            caputure.EditAreaBoxBrush = baseSetting.EditAreaBoxBrush;
            caputure.TextBoxSelectBrush = baseSetting.TextBoxSelectBrush;
            caputure.MouseOverBackground = baseSetting.MouseOverBackground;
            caputure.SelectedBackground = baseSetting.SelectedBackground;
            caputure.ScrolBarTabBrush = baseSetting.ScrolBarTabBrush;
        }

        private void SetResources(string key, object value)
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
    }
}
