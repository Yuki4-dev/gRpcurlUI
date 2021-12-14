using System.Collections;
using System.Collections.Generic;

namespace gRpcurlUI.Model
{
    public interface IReadOnlyAppSetting
    {
        string AppPath { get; }

        double EditFontSize { get; }
    }

    public class AppSetting : Observable, IReadOnlyAppSetting
    {
        private const string KEY_EditFontSize = "EditTextBoxFontSize";

        private const string KEY_FontFamily = "DefaultFontFamily";

        private readonly IDictionary resources = new Dictionary<string, object>();

        private string _AppPath;
        public string AppPath
        {
            get => _AppPath;
            set => OnPropertyChanged(ref _AppPath, value);
        }

        public double EditFontSize
        {
            get => (double)resources[KEY_EditFontSize];
            set
            {
                resources[KEY_EditFontSize] = value;
                OnPropertyChanged();
            }
        }

        public string FontFamily
        {
            get => (string)resources[KEY_FontFamily];
            set
            {
                resources[KEY_FontFamily] = value;
                OnPropertyChanged();
            }
        }

        public AppSetting()
        {
            resources.Add(KEY_EditFontSize, 14.0);
            resources.Add(KEY_FontFamily, "Yu Gothic UI");
        }

        public AppSetting(IDictionary resources)
        {
            this.resources = resources;
        }
    }
}
