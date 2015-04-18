using System;
using System.Windows.Input;

namespace RecordifyAppWin.MainWindowView.Commands
{
    public class MouseUpAction : ICommand
    {
        private MainWindowViewModel viewModel;

        public MouseUpAction(MainWindowViewModel viewModel)
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
            viewModel.MainWindowModel.IsSelected = true;
        }
    }
}