using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using Organizer.Core.Interfaces.ViewModels;
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
                    new FriendsAsyncDataService()), new MeetingAsyncLookupDataService(new MeetingAsyncDataService()), eventAggregator),
                    //() => new FriendDetailViewModel(new FriendsAsyncDataService(), eventAggregator, new MessageDialogService(), new AsyncProgrammingLanguageLookupservice(new ProgrammingLanguagesAsyncDataService())),
                    //() => new MeetingDetailViewModel(eventAggregator, new MessageDialogService(), new MeetingAsyncDataService(), new FriendsAsyncDataService()),
                    new Dictionary<string, Func<IDetailViewModel>>
                    {
                        {typeof(FriendDetailViewModel).Name, ()=> new FriendDetailViewModel(new FriendsAsyncDataService(), eventAggregator, new MessageDialogService(), new AsyncProgrammingLanguageLookupservice(new ProgrammingLanguagesAsyncDataService()))},
                        {typeof(MeetingDetailViewModel).Name, () =>new MeetingDetailViewModel(eventAggregator, new MessageDialogService(), new MeetingAsyncDataService(), new FriendsAsyncDataService())}                                           
                    },
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
