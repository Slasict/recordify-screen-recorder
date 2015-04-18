using System;
using System.Windows;
using System.Windows.Input;

namespace RecordifyAppWin.MainWindowView.Commands
{
    public class ExitApplication : ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}