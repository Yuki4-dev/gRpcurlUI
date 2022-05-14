using gRpcurlUI.Core.Converter.Proto.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace gRpcurlUI.Core.Converter.Proto.Analyze
{
    public class ProtoAnalyzeEntry
    {
        public async Task<ProtoAnalyzeEntryResult> AnalizeAllLineAsync(string[] lines)
        {
            return await Task.Run(() =>
            {
                var errors = new List<ErrorInfomation>();
                var nameScanner = new ProtoNameScanner();
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!nameScanner.ReadLine(lines[i], out var error))
                    {
                        errors.Add(new ErrorInfomation("Name", error, i + 1));
                    }
                }

                var nameInfomation = nameScanner.GetProtoNameInfomatin();
                var messageScanner = new ProtoMessageScanner(nameInfomation);
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!messageScanner.ReadLine(lines[i], out var error))
                    {
                        errors.Add(new ErrorInfomation("Message", error, i + 1));
                    }
                }

                var messageInfomations = messageScanner.GetProtoMessageInfomation();
                var serviceScanner = new ProtoServiceScanner(messageInfomations);
                for (var i = 0; i < lines.Length; i++)
                {
                    if (!serviceScanner.ReadLine(lines[i], out var error))
                    {
                        errors.Add(new ErrorInfomation("Service", error, i + 1));
                    }
                }

                return new ProtoAnalyzeEntryResult(serviceScanner.GetProtoServiceInfomation(), nameInfomation, messageInfomations, errors);
            });
        }
    }

    public class ProtoAnalyzeEntryResult
    {
        public ProtoServiceInfomation ProtoServiceInfomation { get; }

        public ProtoNameInfomatin ProtoNameInfomatin { get; }

        public IEnumerable<ProtoMessageInfomation> ProtoMessageInfomations { get; }

        public IEnumerable<ErrorInfomation> ErrorInfomations { get; }

        public ProtoAnalyzeEntryResult(ProtoServiceInfomation protoServiceInfomation, ProtoNameInfomatin protoNameInfomatin, IEnumerable<ProtoMessageInfomation> protoMessageInfomation, IEnumerable<ErrorInfomation> errorInfomations)
        {
            ProtoServiceInfomation = protoServiceInfomation;
            ProtoNameInfomatin = protoNameInfomatin;
            ProtoMessageInfomations = protoMessageInfomation;
            ErrorInfomations = errorInfomations;
        }
    }

    public class ErrorInfomation
    {
        public string TaskName { get; }

        public string Message { get; }

        public int Line { get; }

        public ErrorInfomation(string taskName, string message, int line)
        {
            TaskName = taskName;
            Message = message;
            Line = line;
        }
    }
}
