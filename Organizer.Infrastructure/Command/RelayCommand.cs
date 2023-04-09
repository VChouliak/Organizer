using System;
using System.Windows.Input;

namespace Organizer.Infrastructure.Command
{
    public class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action<object> _executeMethod;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecute)
        {
            _executeMethod = executeMethod;
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object> executeMethod)
        {
            _executeMethod = executeMethod;
            _canExecute = x => true;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute != null)
            {
                return _canExecute(parameter);
            }
            else
            {
                return false;
            }
        }

        public void Execute(object? parameter)
        {
            _executeMethod(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
