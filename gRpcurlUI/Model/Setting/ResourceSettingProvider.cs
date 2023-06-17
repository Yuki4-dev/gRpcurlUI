using gRpcurlUI.Core.Setting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace gRpcurlUI.Model.Setting
{
    public abstract class ResourceSettingProvider : ISettingValueProvider
    {
        private readonly IDictionary resources;

        private IDictionary? buffer;

        public ResourceSettingProvider()
        {
            resources = App.Current.Resources;
        }

        public ResourceSettingProvider(IDictionary resources)
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
            var newResources = new Dictionary<string, object?>();
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
                SetSetting(key, otherResources[key]);
            }
        }

        public object? GetSetting(string key)
        {
            return resources[key];
        }

        public void SetSetting(string key, object? value)
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
}
