using System;
using System.Diagnostics;
using System.Windows.Input;

namespace RecordifyAppWin.RecManagerWindowView.Commands
{
    public class StartSystemProcess : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter != null && parameter.GetType().Name == "String")
            {
                Process.Start(parameter as string);
            }
        }
    }
}