using System;
using System.Windows.Input;

namespace RecordifyAppWin.RecManagerWindowView.Commands
{
    public class RefreshList : ICommand
    {
        private RecManagerViewModel viewModel;

        public RefreshList(RecManagerViewModel viewModel)
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
            viewModel.PopulateListData();
        }
    }
}