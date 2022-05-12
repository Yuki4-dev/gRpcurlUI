using System;
using System.Collections.Generic;

namespace gRpcurlUI.Core.Converter
{
    public interface ITextToObjectMapConverter : IDisposable
    {
        IDictionary<string, string>? Convert(string text);
    }
}
