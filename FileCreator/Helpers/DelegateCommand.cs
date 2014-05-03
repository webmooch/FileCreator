using System;
using System.Windows.Input;

namespace FileCreator.Helpers
{
    internal class DelegateCommand : ICommand
    {
        private Func<object, bool> canExecute;
        private Action<object> executeAction;
        private bool previousCanExecuteState;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object optionalParameter)
        {
            bool currentCanExecuteState = canExecute(optionalParameter);
            if (currentCanExecuteState != previousCanExecuteState)
            {
                previousCanExecuteState = currentCanExecuteState;
                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, new EventArgs());
            }
            return previousCanExecuteState;
        }

        public event EventHandler CanExecuteChanged;
        public void Execute(object optionalParameter)
        {
            executeAction(optionalParameter);
        }
    }
}
