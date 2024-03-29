﻿using gRpcurlUI.Core.Converter.Proto.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gRpcurlUI.Core.Converter.Proto.Analyze
{
    public class ProtoAnalyzeEntry
    {
        public async Task<ProtoAnalyzeEntryResult> AnalyzeAllLineAsync(string[] lines)
        {
            return await Task.Run(() =>
            {
                var errors = new List<ErrorInformation>();
                var nameScanner = new ProtoNameScanner();
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!nameScanner.ReadLine(lines[i], out var error))
                    {
                        errors.Add(new ErrorInformation("Name", error, i + 1));
                    }
                }

                var nameInformation = nameScanner.GetProtoNameInformation();
                var messageScanner = new ProtoMessageScanner(nameInformation);
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!messageScanner.ReadLine(lines[i], out var error))
                    {
                        errors.Add(new ErrorInformation("Message", error, i + 1));
                    }
                }

                var messageInformation = messageScanner.GetProtoMessageInformation();
                var serviceScanner = new ProtoServiceScanner(messageInformation);
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!serviceScanner.ReadLine(lines[i], out var error))
                    {
                        errors.Add(new ErrorInformation("Service", error, i + 1));
                    }
                }

                return new ProtoAnalyzeEntryResult(serviceScanner.GetProtoServiceInformation(), nameInformation, messageInformation, errors);
            });
        }
    }
}
