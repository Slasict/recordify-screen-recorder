using System.Collections.ObjectModel;
using System.ComponentModel;
using RecordifyAppWin.Recorder.Model;

namespace RecordifyAppWin.RecManagerWindowView
{
    public class RecManagerModel : INotifyPropertyChanged
    {
        private ObservableCollection<RecordingInfo> recordingList;

        public ObservableCollection<RecordingInfo> RecordingList
        {
            get { return recordingList; }
            set
            {
                recordingList = value;
                OnPropertyChanged("RecordingList");
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