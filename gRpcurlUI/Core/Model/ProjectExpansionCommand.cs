using System.Windows.Input;

namespace gRpcurlUI.Core.Model
{
    public class ProjectExpansionCommand
    {
        public string Name { get; }

        public ICommand Command { get; }

        public ProjectExpansionCommand(string name, ICommand command)
        {
            Name = name;
            Command = command;
        }
    }
}
