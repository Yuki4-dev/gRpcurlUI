using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Setting;
using gRpcurlUI.Service;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.Model.Setting
{
    public partial class GrpcurlSettingGroup : ObservableObject, IExpansionSettingGroup
    {
        public string Name => Language.Default.SettingPage.GrpcurlSettingTitle;

        private const string KEY_ExePath = "ExePath";
        public string ExePath
        {
            get
            {
                var value = (string?)settingProvider.GetSetting(KEY_ExePath);
                if (string.IsNullOrEmpty(value))
                {
                    return "grpcurl.exe";
                }
                else
                {
                    return value;
                }
            }
        }

        [ObservableProperty]
        private bool isEnable = false;

        private readonly ObservableCollection<ISettingRow> settingRows = new();
        public ICollection<ISettingRow> SettingRows => settingRows;

        private readonly ApplicationSettingProvider settingProvider;

        public GrpcurlSettingGroup(ApplicationSettingProvider settingProvider)
        {
            this.settingProvider = settingProvider;

            var exePath = new SettingRow(settingProvider, Language.Default.SettingPage.GrpcExePath, KEY_ExePath);
            settingRows.Add(exePath);
        }
    }
}
