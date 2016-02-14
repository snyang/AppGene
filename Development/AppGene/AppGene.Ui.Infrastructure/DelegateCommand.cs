using System;
using System.Windows.Input;

namespace AppGene.Ui.Infrastructure
{
    public class DelegateCommand : ICommand
    {
        private Action executeAction;
        private Func<bool> canExecuteFunction;

        public DelegateCommand(Action executeAction)
            : this(executeAction, null)
        { }

        public DelegateCommand(Action executeAction,
                                        Func<bool> canExecuteFunction)
        {
            this.executeAction = executeAction;
            this.canExecuteFunction = canExecuteFunction;
        }

        private event EventHandler canExecuteChanged;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                canExecuteChanged += value;
            }

            remove
            {
                canExecuteChanged -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteFunction == null)
            {
                return true;
            }

            return canExecuteFunction.Invoke();
        }

        public void Execute(object parameter)
        {
            if (executeAction == null)
            {
                return;
            }

            executeAction.Invoke();
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.canExecuteChanged == null) return;
            this.canExecuteChanged.Invoke(this, new EventArgs());
        }
    }
}