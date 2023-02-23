using System.Windows;
using Organizer.Service.Data;
using Organizer.Service.View;
using Organizer.UI.ViewModel;

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
            var mainWindow = new MainWindow(
                new MainViewModel(new NavigationViewModel(new AsyncFriendLookupService(new FriendsAsyncDataService())), new FriendDetailsViewModel(new FriendsAsyncDataService())));
            mainWindow.Show();
        }
    }
}
