using System.ComponentModel;

namespace RecordifyAppWin.NotificationUserControl
{
    public class NotificationModel : INotifyPropertyChanged
    {
        private string balloonContent;
        private string balloonTitle;

        public string BalloonTitle
        {
            get { return balloonTitle; }
            set
            {
                balloonTitle = value;
                OnPropertyChanged("BalloonTitle");
            }
        }

        public string BalloonContent
        {
            get { return balloonContent; }
            set
            {
                balloonContent = value;
                OnPropertyChanged("BalloonContent");
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