using Organizer.Core.Interfaces.ViewModels;
using Organizer.Data.UnitOfWork;
using Organizer.Infrastructure.Aggregator;
using Organizer.Infrastructure.Data;
using Organizer.Service.Data;
using Organizer.Service.View;
using Organizer.UI.Service;
using Organizer.UI.ViewModel;
using System;
using System.Collections.Generic;

namespace Organizer.UI
{
    //TOTO: adjust and clean
    public class OrganizerMainWindow : MainWindow
    {
        public OrganizerMainWindow() : base(MainViewModel)
        {

        }
        private static MainViewModel MainViewModel 
        {
            get
            {
                var eventAggregator = new EventAggregator();

                var dbContext = new OrganizerContext();
                var unitOfWork = new UnitOfWork(dbContext);

                var friendsAsyncDataService = new FriendsAsyncDataService(unitOfWork);
                var meetingAsyncDataService = new MeetingAsyncDataService(unitOfWork);
                var programmingLanguageAsyncDataService = new ProgrammingLanguagesAsyncDataService(unitOfWork);

                var meetingAsyncLookupDataService = new MeetingAsyncLookupDataService(meetingAsyncDataService);
                var friendAsyncLookupDataService = new AsyncFriendLookupService(friendsAsyncDataService);
                var programmingLanguageAsyncLookupDataService = new AsyncProgrammingLanguageLookupservice(programmingLanguageAsyncDataService);

                var messageDialogService = new MessageDialogService();

                var navigationViewModel = new NavigationViewModel(friendAsyncLookupDataService, meetingAsyncLookupDataService, eventAggregator);

                var detailsViewModelsDictionary = new Dictionary<string, Func<IDetailViewModel>>
                    {
                        {typeof(FriendDetailViewModel).Name, () => new FriendDetailViewModel(friendsAsyncDataService, eventAggregator, messageDialogService, programmingLanguageAsyncLookupDataService)},
                        {typeof(MeetingDetailViewModel).Name, () =>  new MeetingDetailViewModel(eventAggregator, messageDialogService, meetingAsyncDataService, friendsAsyncDataService)},
                        {typeof(ProgrammingLanguageDetailViewModel).Name, () =>  new ProgrammingLanguageDetailViewModel(eventAggregator, messageDialogService,programmingLanguageAsyncDataService)}
                    };
                return new MainViewModel(navigationViewModel, detailsViewModelsDictionary, eventAggregator, messageDialogService);               
            }           
        }
    }
}
