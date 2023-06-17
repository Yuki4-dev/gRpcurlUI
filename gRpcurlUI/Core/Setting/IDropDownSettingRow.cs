using System.Collections.Generic;

namespace gRpcurlUI.Core.Setting
{
    public interface IDropDownSettingRow : ISettingRow
    {
        public ICollection<object> Items { get; }
    }
}
