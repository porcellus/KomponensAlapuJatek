using System.Windows;
using ClientGUI.View;
using ClientGUI.ViewModel;

namespace ClientGUI
{
    public partial class App : Application
    {
        private MainViewModel _mainVm;
        private MainWindow _mainWindow;

        public App()
        {
            _mainVm = new MainViewModel();
            _mainWindow = new MainWindow();
            _mainWindow.Show();
            _mainWindow.DataContext = _mainVm;
            _mainWindow.Closed += OnMainWindowClosed;
        }

        void OnMainWindowClosed(object sender, System.EventArgs e)
        {
            Shutdown(0);
        }
    }
}
