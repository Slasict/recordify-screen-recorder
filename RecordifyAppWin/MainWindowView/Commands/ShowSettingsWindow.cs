using System;
using System.Windows;
using System.Windows.Input;

namespace RecordifyAppWin.MainWindowView.Commands
{
    public class ShowSettingsWindow : ICommand
    {
        private MainWindowViewModel viewModel;

        public ShowSettingsWindow(MainWindowViewModel viewModel)
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
            if (!viewModel.IsWindowOpen<SettingsWindow>())
            {
                new SettingsWindow
                {
                    Visibility = Visibility.Visible
                };
            }
        }
    }
}