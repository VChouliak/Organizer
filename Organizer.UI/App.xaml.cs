using Organizer.Service.Data;
using System.Windows;

namespace Organizer.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //TODO: Refactor
            var mainWindow = new MainWindow(new ViewModel.MainViewModel(new FriendsAsyncDataService()));
            mainWindow.Show();
        }
    }
}
