using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RecordifyAppWin.MainWindowView.Commands;

namespace RecordifyAppWin.MainWindowView
{
    public class MainWindowViewModel
    {
        private List<ContextMenuItemModel> contextMenuItems;
        private MainWindowModel model;
        private Rect screenSize;

        public MainWindowViewModel()
        {
            model = new MainWindowModel();

            SelectionActionCommand = new SelectionAction(this);
            ShowRecordManagerCommand = new ShowRecordManager(this);
            ShowSettingsFormCommand = new ShowSettingsWindow(this);
            ExitApplicationCommand = new ExitApplication();
            MouseDownCommand = new MouseDownAction(this);
            MouseMoveCommand = new MouseMoveAction(this);
            MouseUpCommand = new MouseUpAction(this);

            contextMenuItems = new List<ContextMenuItemModel>
            {
                new ContextMenuItemModel("Record", SelectionActionCommand),
                new ContextMenuItemModel("My Recordings", ShowRecordManagerCommand),
                new ContextMenuItemModel("Settings", ShowSettingsFormCommand),
                new ContextMenuItemModel("Exit", ExitApplicationCommand)
            };

            model.RecordButton = new RecordButtonParams {Width = 120, Height = 38};

            screenSize = new Rect
            {
                Width = SystemParameters.PrimaryScreenWidth,
                Height = SystemParameters.PrimaryScreenHeight
            };
        }

        public MainWindowModel MainWindowModel
        {
            get { return model; }
        }

        public IList<ContextMenuItemModel> ContextMenuItems
        {
            get { return contextMenuItems; }
        }


        public ICommand SelectionActionCommand { get; private set; }

        public ICommand ShowRecordManagerCommand { get; private set; }

        public ICommand ShowSettingsFormCommand { get; private set; }

        public ICommand ExitApplicationCommand { get; private set; }

        public ICommand MouseDownCommand { get; private set; }

        public ICommand MouseMoveCommand { get; private set; }

        public ICommand MouseUpCommand { get; private set; }

        public void ShowSelection()
        {
            model.SelectionVisibility = Visibility.Visible;
            model.Foreground = "#01000000";
            model.Background = "#01000000";
        }

        public void HideSelection()
        {
            model.Width = 0;
            model.Height = 0;
            model.Leftoffset = 0;
            model.Topoffset = 0;
            model.StartPos = new Point(0, 0);
            model.RecordButton.IsVisible = false;
            model.Foreground = "#00000000";
            model.Background = "#00000000";
            model.Stroke = "Blue";
            model.SelectionVisibility = Visibility.Hidden;
        }

        public Thickness GetPossibleBtnLoc()
        {
            var thickness = new Thickness();

            if (model.Topoffset + model.Height + model.RecordButton.Height < screenSize.Height)
            {
                thickness.Left = (model.Leftoffset + model.Width / 2) - (model.RecordButton.Width / 2);
                thickness.Top = model.Topoffset + model.Height + 5;
            }
            else if (model.Leftoffset - model.RecordButton.Width > 0)
            {
                thickness.Left = model.Leftoffset - model.RecordButton.Width - 5;
                thickness.Top = (model.Topoffset + model.Height / 2) - (model.RecordButton.Height / 2);
            }
            else if (model.Leftoffset + model.Width + model.RecordButton.Width < screenSize.Width)
            {
                thickness.Left = model.Leftoffset + model.Width + 5;
                thickness.Top = (model.Topoffset + model.Height / 2) - (model.RecordButton.Height / 2);
            }
            else if (model.Topoffset - model.RecordButton.Height > 0)
            {
                thickness.Left = (model.Leftoffset + model.Width / 2) - (model.RecordButton.Width / 2);
                thickness.Top = model.Topoffset - model.RecordButton.Height - 5;
            }
            else
            {
                thickness.Left = (model.Leftoffset + model.Width / 2) - (model.RecordButton.Width / 2);
                thickness.Top = model.Topoffset + model.Height - model.RecordButton.Height - 5;
            }
            return thickness;
        }

        public bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
                ? Application.Current.Windows.OfType<T>().Any()
                : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}