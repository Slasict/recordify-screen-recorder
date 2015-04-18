using System.ComponentModel;
using System.Windows;

namespace RecordifyAppWin.MainWindowView
{

    public class RecordButtonParams : INotifyPropertyChanged
    {

        private Thickness position;
        private int width, height;
        private bool isVisible;

        public Thickness Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                OnPropertyChanged("Position");
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                OnPropertyChanged("IsVisible");
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

    public class MainWindowModel : INotifyPropertyChanged
    {

        private bool isRecording, isSelected;
        private Visibility selectionVisibility = Visibility.Hidden;
        private double leftOffset, topOffset, width, height;
        private Point startPos;
        private string foreground = "#01000000";
        private string background = "#01000000";
        private string stroke = "Blue";
        private RecordButtonParams recordButton;

        public bool IsRecording
        {
            get
            {
                return isRecording;
            }
            set
            {
                isRecording = value;
                OnPropertyChanged("IsRecording");
            }
        }

        public Visibility SelectionVisibility
        {
            get
            {
                return selectionVisibility;
            }
            set
            {
                selectionVisibility = value;
                OnPropertyChanged("SelectionVisibility");
            }
        }

        public double Leftoffset
        {
            get
            {
                return leftOffset;
            }
            set
            {
                leftOffset = value;
                OnPropertyChanged("Leftoffset");
            }
        }

        public double Topoffset
        {
            get
            {
                return topOffset;
            }
            set
            {
                topOffset = value;
                OnPropertyChanged("Topoffset");
            }
        }

        public double Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        public double Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        public Point StartPos
        {
            get
            {
                return startPos;
            }
            set
            {
                startPos = value;
                OnPropertyChanged("StartPos");
            }
        }

        public string Foreground
        {
            get
            {
                return foreground;
            }
            set
            {
                foreground = value;
                OnPropertyChanged("Foreground");
            }
        }

        public string Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                OnPropertyChanged("Background");
            }
        }

        public string Stroke
        {
            get
            {
                return stroke;
            }
            set
            {
                stroke = value;
                OnPropertyChanged("Stroke");
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public RecordButtonParams RecordButton
        {
            get
            {
                return recordButton;
            }
            set
            {
                recordButton = value;
                OnPropertyChanged("RecordButton");
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
