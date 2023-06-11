using System;

namespace gRpcurlUI.Core.Model
{
    public interface IJsonObject
    {
        Type JsonType { get; }

        object ToJsonObject();

        void LoadJsonObject(object jsonObject);
    }

    public abstract class JsonObject : IJsonObject
    {
        public Type JsonType => GetType();

        public object ToJsonObject()
        {
            return this;
        }

        public abstract void LoadJsonObject(object jsonObject);
    }
}
