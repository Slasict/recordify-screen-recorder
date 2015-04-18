using System;
using System.Collections.Generic;
using System.ComponentModel;
using RecordifyAppWin.Recorder.Model;

namespace RecordifyAppWin.VideoProcessWindowView
{
    public enum ProgressState
    {
        Normal,
        Failed
    }

    public class VideoProcessModel : INotifyPropertyChanged
    {
        private double currentProgress;
        private string header = "Processing your recording...";
        private string progressColor = "LimeGreen";
        private string taskbarProgressState = "Normal";
        private List<TodoListItem> todoList;

        public List<TodoListItem> TodoList
        {
            get { return todoList; }
            set
            {
                todoList = value;
                OnPropertyChanged("TodoList");
            }
        }

        public double CurrentProgress
        {
            get { return currentProgress; }
            set
            {
                Console.WriteLine(value);
                currentProgress = value;
                OnPropertyChanged("CurrentProgress");
                OnPropertyChanged("CurrentProgressTaskbarValue");
            }
        }

        public string ProgressColor
        {
            get { return progressColor; }
            set
            {
                progressColor = value;
                OnPropertyChanged("ProgressColor");
            }
        }

        public string TaskbarProgressState
        {
            get { return taskbarProgressState; }
            set
            {
                taskbarProgressState = value;
                OnPropertyChanged("TaskbarProgressState");
            }
        }

        public string Header
        {
            get { return header; }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }

        public double CurrentProgressTaskbarValue
        {
            get { return currentProgress / 100; }
        }

        public ProgressState ProgressState
        {
            set
            {
                if (value == ProgressState.Normal)
                {
                    TaskbarProgressState = "Normal";
                    ProgressColor = "LimeGreen";
                }
                else if (value == ProgressState.Failed)
                {
                    TaskbarProgressState = "Error";
                    ProgressColor = "Red";
                }
            }
        }

        public RecordingInfo RecordingInfo { get; set; }


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