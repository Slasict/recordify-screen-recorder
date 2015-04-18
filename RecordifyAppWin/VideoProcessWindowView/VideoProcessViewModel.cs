using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using RecordifyAppWin.RecManagerWindowView;
using RecordifyAppWin.RecManagerWindowView.Commands;

namespace RecordifyAppWin.VideoProcessWindowView
{
    public class VideoProcessViewModel
    {
        private Converter converter;
        private Uploader uploader;
        private BackgroundWorker worker;

        public VideoProcessViewModel()
        {
            Model = new VideoProcessModel();
            Model.TodoList = new List<TodoListItem>
            {
                new TodoListItem("Combining video and audio"),
                new TodoListItem("Encoding .webm"),
                new TodoListItem("Generating GIF"),
                new TodoListItem("Uploading...")
            };
            ProcessStartCommand = new StartSystemProcess();
        }

        public VideoProcessModel Model { get; set; }
        public ICommand ProcessStartCommand { get; private set; }

        public void StartProcess()
        {
            if (Model.RecordingInfo == null) return;

            TodoListItem tiCombineAudio = Model.TodoList[0];
            TodoListItem tiEncodeWebm = Model.TodoList[1];
            TodoListItem tiConvertGif = Model.TodoList[2];
            TodoListItem tiUpload = Model.TodoList[3];

            worker = new BackgroundWorker {WorkerSupportsCancellation = true};
            worker.DoWork += (ws, we) =>
            {
                double progressLimitForEachTodo = 100.0 / Model.TodoList.Count;
                converter = new Converter(Model.RecordingInfo.Path);
                converter.Progress += (s, e) =>
                {
                    double currentTodoProgressPercent = Model.CurrentProgress - (((int) (Model.CurrentProgress / progressLimitForEachTodo)) * progressLimitForEachTodo);
                    Model.CurrentProgress += ((progressLimitForEachTodo / 100 * e.DonePercentage) - currentTodoProgressPercent) * (progressLimitForEachTodo / 100);
                };

                // combine audio and video
                tiCombineAudio.State = TodoListItemState.Processing;
                converter.Combine();
                tiCombineAudio.State = TodoListItemState.Finished;
                Model.CurrentProgress = progressLimitForEachTodo;

                // convert to webm
                tiEncodeWebm.State = TodoListItemState.Processing;
                converter.ToWebm();
                if (converter.FinishedGood)
                {
                    tiEncodeWebm.State = TodoListItemState.Finished;
                    tiEncodeWebm.Text = tiEncodeWebm.Text + " (" + Model.RecordingInfo.Path + ".webm)";
                    Model.RecordingInfo.Formats.Add("webm");
                }
                else
                {
                    tiEncodeWebm.State = TodoListItemState.Failed;
                }
                Model.CurrentProgress = progressLimitForEachTodo * 2;

                // generate gif
                tiConvertGif.State = TodoListItemState.Processing;
                if (Model.RecordingInfo.Duration <= 30)
                {
                    converter.ToGif();
                    if (converter.FinishedGood)
                    {
                        tiConvertGif.State = TodoListItemState.Finished;
                        tiConvertGif.Text = tiConvertGif.Text + " (" + Model.RecordingInfo.Path + ".gif)";
                        Model.RecordingInfo.Formats.Add("gif");
                    }
                    else
                    {
                        tiConvertGif.State = TodoListItemState.Failed;
                    }
                }
                else
                {
                    tiConvertGif.Text = tiConvertGif.Text + " (skipped. limit 30 second)";
                    tiConvertGif.State = TodoListItemState.Failed;
                }
                Model.CurrentProgress = progressLimitForEachTodo * 3;

                // upload
                tiUpload.State = TodoListItemState.Processing;
                try
                {
                    uploader = new Uploader(Model.RecordingInfo);
                    uploader.Progress += (s, e) =>
                    {
                        Model.CurrentProgress += (progressLimitForEachTodo / 100 * e.UploadedPercentage) -
                                                 Model.CurrentProgress + (progressLimitForEachTodo * 2);
                    };
                    uploader.StartSync();
                    tiUpload.State = TodoListItemState.Finished;
                    JObject uploaderResponse = uploader.JsonResponse;
                    Model.RecordingInfo.Url = uploaderResponse["url"].ToString();
                    Model.RecordingInfo.ActionKey = uploaderResponse["actionKey"].ToString();
                }
                catch (Exception e)
                {
                    tiUpload.State = TodoListItemState.Failed;
                    tiUpload.Text = tiUpload.Text + " " + e.Message;
                    Model.ProgressState = ProgressState.Failed;
                }
                Model.CurrentProgress = 100;
            };

            worker.RunWorkerCompleted += (s, e) =>
            {
                string headerText = "Generated: MP4";
                if (tiEncodeWebm.State == TodoListItemState.Finished)
                {
                    headerText += ", WEBM";
                }
                if (tiConvertGif.State == TodoListItemState.Finished)
                {
                    headerText += ", GIF";
                }
                if (tiUpload.State == TodoListItemState.Failed)
                {
                    headerText = " Upload failed.";
                    Model.RecordingInfo.Url = "Not uploaded";
                }
                else
                {
                    var link = new Hyperlink {NavigateUri = new Uri(Model.RecordingInfo.Url)};
                    link.Inlines.Add(Model.RecordingInfo.Url);
                    headerText = " Uploaded: " + link;
                }
                Model.Header = headerText;
                new RecManagerService().AddRecording(Model.RecordingInfo);
            };

            worker.RunWorkerAsync();
        }
    }
}