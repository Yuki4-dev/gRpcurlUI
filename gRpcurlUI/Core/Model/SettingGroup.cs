using System.Collections.Generic;
using System.ComponentModel;

namespace gRpcurlUI.Core.Model
{
    public interface ISettingGroup : INotifyPropertyChanged
    {
        public string Name { get; }

        public ICollection<ISettingRow> SettingRows { get; }
    }

    public interface IExpansionSettingGroup : ISettingGroup
    {
        public bool IsEnable { get; }
    }
}
