using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Globalization;
using System.ComponentModel;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RecordifyAppWin.NotificationUserControl;
using RecordifyAppWin.Recorder.Model;

namespace RecordifyAppWin
{

    public class UploaderFileModel
    {
        public UploaderFileModel()
        {
            ContentType = "application/octet-stream";
        }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }

    public class UploaderProgress
    {
        public double UploadedBytes { get; set; }
        public double UploadedPercentage { get; set; }
        public UploaderProgress(double uploadedBytes, double uploadedPercentage)
        {
            UploadedBytes = uploadedBytes;
            UploadedPercentage = uploadedPercentage;
        }
    }

    public class Uploader
    {
        private const string WebEndUrl = @"http://localhost/recordify/upload";
        private WebRequest WebRequest { get; set; }
        private string WebRequestBoundary { get; set; }
        public JObject JsonResponse { get; set; }
        private List<UploaderFileModel> Files { get; set; }
        private NameValueCollection Values { get; set; }
        private long totalFileBytes, totalUploadedBytes;
        private RecordingInfo RecordingInfo { get; set; }
        public BackgroundWorker Worker;
        public EventHandler<UploaderProgress> Progress;

        public Uploader(RecordingInfo recordingInfo)
        {
            RecordingInfo = recordingInfo;
            
            Files = new List<UploaderFileModel>();
            foreach (string format in RecordingInfo.Formats)
            {
                try
                {
                    UploaderFileModel uFile = new UploaderFileModel();
                    uFile.Filename = RecordingInfo.Path + "." + format;
                    uFile.Stream = File.Open(RecordingInfo.Path + "." + format, FileMode.Open, FileAccess.Read);
                    Files.Add(uFile);
                    totalFileBytes += uFile.Stream.Length;
                }
                catch
                {                    
                    string format1 = format;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Notification.Instance.ShowNotificationBalloon("File error", "Error loading " + RecordingInfo.Path + "." + format1 + " for upload.");
                    });
                }
            }

            /*if (RecordingInfo.Recorder == "VLC")
            {
                UploaderFileModel audioFile = new UploaderFileModel
                {
                    Filename = RecordingInfo.Path + ".mp3",
                    Stream = File.Open(RecordingInfo.Path + ".mp3", FileMode.Open, FileAccess.Read),
                };
                Files.Add(audioFile);
            }*/

            Values = new NameValueCollection
            {
                { "name", RecordingInfo.Name },
                { "duration", RecordingInfo.Duration.ToString() },
                { "dimension", RecordingInfo.Dimension.ToString() },
                { "date", RecordingInfo.Date.ToString("yyyy-MM-dd H:mm:ss") },
                { "recorder", RecordingInfo.Recorder },
            };
            Worker = new BackgroundWorker();
            InitializeWebRequest();
        }

        private void InitializeWebRequest()
        {
            WebRequest = WebRequest.Create(WebEndUrl);
            WebRequest.Method = "POST";
            WebRequest.ContentType = "application/json; charset=utf-8";
            WebRequestBoundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
            WebRequest.ContentType = "multipart/form-data; boundary=" + WebRequestBoundary;
            WebRequestBoundary = "--" + WebRequestBoundary;
        }

        public void StartAsync()
        {
            Worker.DoWork += DoUpload;
            Worker.RunWorkerAsync();
        }

        public void StartSync()
        {
            DoUpload(null, EventArgs.Empty);
        }

        public void Stop()
        {
            if (WebRequest != null)
            {
                WebRequest.Abort();
                WebRequest = null;
            }
        }


        private void DoUpload(object sender, EventArgs e)
        {
            try
            {
                using (Stream requestStream = WebRequest.GetRequestStream())
                {
                    
                    foreach (string name in Values.Keys)
                    {
                        var buffer = Encoding.ASCII.GetBytes(WebRequestBoundary + Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.UTF8.GetBytes(Values[name] + Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                    }
                    
                    foreach (UploaderFileModel file in Files)
                    {
                        var buffer = Encoding.ASCII.GetBytes(WebRequestBoundary + Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.UTF8.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"{2}", file.Filename, file.Filename, Environment.NewLine));
                        requestStream.Write(buffer, 0, buffer.Length);
                        buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", file.ContentType, Environment.NewLine));
                        requestStream.Write(buffer, 0, buffer.Length);
                        var fileReadBuffer = new byte[1024 * 5];
                        int bytesRead;
                        while ((bytesRead = file.Stream.Read(fileReadBuffer, 0, fileReadBuffer.Length)) != 0)
                        {
                            totalUploadedBytes += bytesRead;
                            requestStream.Write(fileReadBuffer, 0, bytesRead);
                            OnProgress(new UploaderProgress(totalUploadedBytes, 100.0 / totalFileBytes * totalUploadedBytes));
                        }
                        buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                        requestStream.Write(buffer, 0, buffer.Length);
                        file.Stream.Close();
                    }

                    var boundaryBuffer = Encoding.ASCII.GetBytes(WebRequestBoundary + "--");
                    requestStream.Write(boundaryBuffer, 0, boundaryBuffer.Length);
                }

                using (var response = WebRequest.GetResponse())
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    JsonResponse = (JObject) JsonConvert.DeserializeObject(reader.ReadToEnd());   
                }
            }
            finally
            {
                WebRequest.Abort();
                foreach (var file in Files)
                {
                    file.Stream.Close();
                }
            }
        }

        public void OnProgress(UploaderProgress args)
        {
            EventHandler<UploaderProgress> handler = Progress;
            if (handler != null)
            {
                handler(this, args);
            }
        }

    }
}
