using System;
using System.Windows;
using Organizer.Infrastructure.Aggregator;
using Organizer.Service.Data;
using Organizer.Service.View;
using Organizer.UI.Service;
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
            var eventAggregator = new EventAggregator();
            var mainWindow = new MainWindow(
                new MainViewModel(new NavigationViewModel(new AsyncFriendLookupService(
                    new FriendsAsyncDataService()), eventAggregator), 
                    () => new FriendDetailsViewModel(new FriendsAsyncDataService(), eventAggregator, new MessageDialogService()),
                    eventAggregator,
                    new MessageDialogService()
                    ));

            mainWindow.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unexpected error occured. Please inform the admin." + Environment.NewLine + e.Exception.Message, "Unexpected error");
        }
    }
}
