using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using RecordifyAppWin.NotificationUserControl;
using RecordifyAppWin.Recorder.Model;

namespace RecordifyAppWin.RecManagerWindowView
{
    public class RecManagerService
    {
        private const string JsonPath = @".\recordings.json";
        private RecManagerModel model;

        public RecManagerService()
        {
        }

        public RecManagerService(RecManagerModel model)
        {
            this.model = model;
        }

        public ObservableCollection<RecordingInfo> GetRecordings()
        {
            if (File.Exists(JsonPath))
            {
                using (StreamReader sr = new StreamReader(JsonPath))
                {
                    var reader = new JsonTextReader(sr);
                    var serializer = new JsonSerializer();
                    var deserialized = serializer.Deserialize(reader);
                    if (deserialized != null)
                    {
                        string parsedData = deserialized.ToString();
                        return JsonConvert.DeserializeObject<ObservableCollection<RecordingInfo>>(parsedData);
                    }
                }
            }
            return new ObservableCollection<RecordingInfo>();
        }

        public void DeleteRecording(RecordingInfo recInfo)
        {
            string path = recInfo.Path;
            // remove files
            if (File.Exists(path + ".mp4")) File.Delete(path + ".mp4");
            if (File.Exists(path + ".webm")) File.Delete(path + ".webm");

            // remove entry from json file
            ObservableCollection<RecordingInfo> recordings = GetRecordings();
            using (StreamWriter writer = new StreamWriter(JsonPath))
            {
                try
                {
                    var itemToRemove = recordings.Single(r => r.Url == recInfo.Url);
                    recordings.Remove(itemToRemove);
                    writer.WriteLine(JsonConvert.SerializeObject(recordings, Formatting.Indented));
                }
                catch (Exception ex)
                {
                    Notification.Instance.ShowBalloonyTip("Delete local file failed.", ex.Message);
                }
            }

            // remove remotely
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["actionKey"] = recInfo.ActionKey;
                try
                {
                    wb.UploadValues(recInfo.Url, data);
                }
                catch (Exception ex)
                {
                    Notification.Instance.ShowBalloonyTip("Remote file deletion failed.", ex.Message);
                }
            }
            if (model != null)
            {
                model.RecordingList = recordings;
            }
        }

        public void AddRecording(RecordingInfo recInfo)
        {
            ObservableCollection<RecordingInfo> recordings = GetRecordings();
            using (StreamWriter writer = new StreamWriter(JsonPath, true))
            {
                recordings.Add(recInfo);
                writer.WriteLine(JsonConvert.SerializeObject(recordings, Formatting.Indented));
            }
            if (model != null)
            {
                model.RecordingList = recordings;
            }
        }

    }
}
