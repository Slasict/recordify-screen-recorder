using System.ComponentModel;

namespace RecordifyAppWin.VideoProcessWindowView
{
    public enum TodoListItemState
    {
        Finished,
        Processing,
        Waiting,
        Failed
    }

    public class TodoListItem : INotifyPropertyChanged
    {
        private TodoListItemState state = TodoListItemState.Waiting;
        private string icon, text;

        public TodoListItem(string text)
        {
            Text = text;
            Icon = "Resources/waiting.ico";
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

        public string Icon
        {
            get {
                return icon;
            }
            set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        public TodoListItemState State
        {
            get { return state; }
            set
            {
                state = value;
                OnPropertyChanged("State");
                if (value == TodoListItemState.Finished)
                {
                    Icon = "Resources/finished.ico";
                }
                if (value == TodoListItemState.Processing)
                {
                    Icon = "Resources/processing.ico";
                }
                if (value == TodoListItemState.Waiting)
                {
                    Icon = "Resources/waiting.ico";
                }
                if (value == TodoListItemState.Failed)
                {
                    Icon = "Resources/error.ico";
                }
            }
        }

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
