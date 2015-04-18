using System.Windows.Input;
using RecordifyAppWin.RecManagerWindowView.Commands;

namespace RecordifyAppWin.RecManagerWindowView
{
    public class RecManagerViewModel
    {
        private RecManagerModel model;
        private RecManagerService service;

        public RecManagerViewModel()
        {
            model = new RecManagerModel();
            service = new RecManagerService(model);
            PopulateListData();
            ListViewSizeChangedCommand = new ListViewSizeChanged();
            ProcessStartCommand = new StartSystemProcess();
            DeleteRecordingCommand = new DeleteRecording(this);
            RefreshCommand = new RefreshList(this);
        }

        public RecManagerModel RecManagerModel
        {
            get { return model; }
        }

        public RecManagerService RecManagerService
        {
            get { return service; }
        }

        public ICommand ListViewSizeChangedCommand { get; private set; }

        public ICommand ProcessStartCommand { get; private set; }

        public ICommand DeleteRecordingCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        public void PopulateListData()
        {
            RecManagerModel.RecordingList = RecManagerService.GetRecordings();
        }
    }
}