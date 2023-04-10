using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models.Lookups;
using Organizer.Core.ViewModel;
using Organizer.UI.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IAsyncLookupService<LookupItem> _lookupService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IAsyncLookupService<LookupItem> lookupService, IEventAggregator eventAggregator)
        {
            _lookupService = lookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();

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
            var lookup = await _lookupService.GetLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(FriendDetailsViewModel), _eventAggregator));
            }

        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        private void AfterDetailSaved(AfterDetailSavedEventArgs eventargs)
        {
            switch (eventargs.ViewModel)
            {
                case nameof(FriendDetailsViewModel):

                    var lookupItem = Friends.SingleOrDefault(f => f.Id == eventargs.Id);
                    if (lookupItem == null)
                    {
                        Friends.Add(new NavigationItemViewModel(eventargs.Id, eventargs.DisplayMember, nameof(FriendDetailsViewModel), _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = eventargs.DisplayMember;
                    }
                    break;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs friendItemViewModel)
        {
            switch (friendItemViewModel.ViewModel)
            {
                case nameof(FriendDetailsViewModel):
                    var friend = Friends.SingleOrDefault(friend => friend.Id == friendItemViewModel.Id);
                    if (friend != null)
                    {
                        Friends.Remove(friend);
                    }
                    break;
            }
        }

    }
}
