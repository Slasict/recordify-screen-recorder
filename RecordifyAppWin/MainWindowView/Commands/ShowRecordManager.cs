using System;
using System.Windows;
using System.Windows.Input;

namespace RecordifyAppWin.MainWindowView.Commands
{
    public class ShowRecordManager : ICommand
    {
        private MainWindowViewModel viewModel;

        public ShowRecordManager(MainWindowViewModel viewModel)
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
            if (!viewModel.IsWindowOpen<RecordManager>())
            {
                new RecordManager
                {
                    Visibility = Visibility.Visible
                };
            }
        }
    }
}