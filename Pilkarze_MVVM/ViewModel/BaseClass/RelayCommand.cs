using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Pilkarze_MVVM.ViewModel.BaseClass
{
    class RelayCommand : ICommand
    {
        #region pola prywatne
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion

        #region konstruktor
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            else
                _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region Składowe interfejsu ICommand
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null) CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null) CommandManager.RequerySuggested -= value;
            }
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion
    }
}
