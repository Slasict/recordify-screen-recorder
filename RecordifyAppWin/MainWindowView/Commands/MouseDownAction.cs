using System;
using System.Windows;
using System.Windows.Input;

namespace RecordifyAppWin.MainWindowView.Commands
{
    public class MouseDownAction : ICommand
    {
        private MainWindowViewModel viewModel;

        public MouseDownAction(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            viewModel.MainWindowModel.StartPos = Mouse.GetPosition(Application.Current.MainWindow);
        }
    }
}