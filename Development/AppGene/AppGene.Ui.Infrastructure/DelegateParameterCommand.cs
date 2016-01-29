using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppGene.Ui.Infrastructure
{
    public class DelegateParameterCommand : ICommand
    {
        private Action<object> executeAction;
        private Func<object, bool> canExecuteFunction;

        public DelegateParameterCommand(Action<object> executeAction)
            : this(executeAction, null)
        { }

        public DelegateParameterCommand(Action<object> executeAction,
                                        Func<object, bool> canExecuteFunction)
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

            return canExecuteFunction.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
           if (executeAction == null)
            {
                return;
            }

            executeAction.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (this.canExecuteChanged == null) return;
            this.canExecuteChanged.Invoke(this, new EventArgs());
        }
    }
}
