using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models.Lookups;
using Organizer.Core.ViewModel;
using Organizer.UI.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IAsyncLookupService<LookupItem> _friendLookupService;
        private readonly IAsyncLookupService<LookupItem> _meetingLookupService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IAsyncLookupService<LookupItem> friendLookupService, IAsyncLookupService<LookupItem> meetingLookupService, IEventAggregator eventAggregator)
        {
            _friendLookupService = friendLookupService;
            _meetingLookupService = meetingLookupService;
            _eventAggregator = eventAggregator;

            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();

            _eventAggregator.Subscribe<AfterDetailSavedEventArgs>(eventargs =>
            {
                AfterDetailSaved(eventargs);
            });

            _eventAggregator.Subscribe<AfterDetailDeletedEventArgs>(eventargs =>
            {
                AfterDetailDeleted(eventargs);
            });
        }

        public async Task LoadAsync()
        {
            var friendsLookup = await _friendLookupService.GetLookupAsync();
            Friends.Clear();
            foreach (var item in friendsLookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(FriendDetailViewModel), _eventAggregator));
            }

            var meetingsLookup = await _meetingLookupService.GetLookupAsync();
            Meetings.Clear();
            foreach (var item in meetingsLookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(MeetingDetailViewModel), _eventAggregator));
            }


        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }
        private void AfterDetailSaved(AfterDetailSavedEventArgs eventargs)
        {
            switch (eventargs.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, eventargs);
                    break;

                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, eventargs);
                    break;
            }
        }
        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs eventargs)
        {
            var lookupItem = items.SingleOrDefault(f => f.Id == eventargs.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(eventargs.Id, eventargs.DisplayMember, eventargs.ViewModelName, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = eventargs.DisplayMember;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs eventargs)
        {
            switch (eventargs.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, eventargs);
                    break;

                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, eventargs);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs eventargs)
        {
            var item = items.SingleOrDefault(friend => friend.Id == eventargs.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }
    }
}
