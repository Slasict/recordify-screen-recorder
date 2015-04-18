namespace RecordifyAppWin.NotificationUserControl
{
    public class NotificationViewModel
    {
        public NotificationViewModel()
        {
            Model = new NotificationModel();
        }

        public NotificationModel Model { get; set; }
    }
}