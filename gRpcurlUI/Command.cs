using System;
using System.Windows.Input;

namespace gRpcurlUI
{
    public class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<object?> command;

        private bool canExecuteValue = true;
        public bool CanExecuteValue
        {
            get => canExecuteValue;
            set
            {
                if (canExecuteValue != value)
                {
                    canExecuteValue = value;
                    CanExecuteChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public Command(Action action) : this((p) => action.Invoke()) { }

        public Command(Action<object?> action)
        {
            command = action;
        }

        public bool CanExecute(object? parameter)
        {
            return CanExecuteValue;
        }

        public void Execute(object? parameter)
        {
            command(parameter);
        }

        public static Command<T> Create<T>(Action<T?> action)
        {
            return new Command<T>(action);
        }

        public static void ChangeCanExecute(bool canExecute, params ICommand[] commands)
        {
            foreach (var command in commands)
            {
                if (command is Command c)
                {
                    c.CanExecuteValue = canExecute;
                }
            }
        }
    }

    public class Command<T> : Command
    {
        public Command(Action<T?> action) : base((p) => action((T?)p)) { }
    }

}
