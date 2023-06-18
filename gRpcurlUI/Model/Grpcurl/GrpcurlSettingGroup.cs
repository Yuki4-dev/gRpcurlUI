using CommunityToolkit.Mvvm.ComponentModel;
using gRpcurlUI.Core.Setting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace gRpcurlUI.Model.Setting
{
    public partial class GrpcurlSettingGroup : ObservableObject, IExpansionSettingGroup
    {
        public string Name => Language.Default.SettingPage.GrpcurlSettingTitle;

        private const string KEY_ExePath = "ExePath";
        public string ExePath => settingProvider.GetSettingValue(KEY_ExePath) ?? "grpcurl.exe";

        [ObservableProperty]
        private bool isEnable = false;

        private readonly ObservableCollection<ISettingRow> settingRows = new();
        public ICollection<ISettingRow> SettingRows => settingRows;

        private readonly IApplicationSetting settingProvider;

        public GrpcurlSettingGroup(IApplicationSetting settingProvider)
        {
            this.settingProvider = settingProvider;

            var exePath = new SettingRow(new ApplicationSettingValueProvider(settingProvider), Language.Default.SettingPage.GrpcExePath, KEY_ExePath);
            settingRows.Add(exePath);
        }
    }
}
