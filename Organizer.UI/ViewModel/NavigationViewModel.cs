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
            
            _eventAggregator.Subscribe<OnSaveFriendEventArgs>(f =>
            {
                AfterFriendSaved(f);
            });
            
            _eventAggregator.Subscribe<OnDeleteFriendEventArgs>(f =>
            {
                AfterFriendDeleted(f);
            });
        }

        public async Task LoadAsync()
        {
            var lookup = await _lookupService.GetLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }

        }

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        private void AfterFriendSaved(OnSaveFriendEventArgs friend)
        {
            var lookupItem = Friends.SingleOrDefault(f => f.Id == friend.Id);
            if (lookupItem == null)
            {
                Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = friend.DisplayMember;
            }
        }

        private void AfterFriendDeleted(OnDeleteFriendEventArgs friendItemViewModel)
        {
            var friend = Friends.SingleOrDefault(friend => friend.Id == friendItemViewModel.Id);
            if (friend != null)
            {
                Friends.Remove(friend);
            }
        }

    }
}
