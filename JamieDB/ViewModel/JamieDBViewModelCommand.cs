using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JamieDB.ViewModel
{
    class JamieDBViewModelCommand : ICommand
    {
        public JamieDBViewModelCommand(JamieDBViewModelCommandCanExecute CanExecuteCommand, JamieDBViewModelCommandExecute ExecuteCommand)
        {
            CanExecuteMethod += CanExecuteCommand;
            ExecuteMethod += ExecuteCommand;

        }

        public delegate bool JamieDBViewModelCommandCanExecute(object parameter);
        public delegate void JamieDBViewModelCommandExecute(object parameter);

        public event EventHandler CanExecuteChanged;

        JamieDBViewModelCommandCanExecute CanExecuteMethod;
        JamieDBViewModelCommandExecute ExecuteMethod;

        public bool CanExecute(object parameter)
        {
            if (CanExecuteMethod != null) { return CanExecuteMethod(parameter); }
            else return true;
        }

        public void Execute(object parameter)
        {
            ExecuteMethod?.Invoke(parameter);
            // Vereinfachte Syntax für
            //if (ExecuteMethod != null) ExecuteMethod(parameter);

        }
    }
}

