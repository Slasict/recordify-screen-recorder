using System.ComponentModel;
using System.Windows.Input;

namespace RecordifyAppWin.MainWindowView
{
    public class ContextMenuItemModel : INotifyPropertyChanged
    {
        private string text;

        public ContextMenuItemModel(string text, ICommand command)
        {
            Text = text;
            Command = command;
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public ICommand Command { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}