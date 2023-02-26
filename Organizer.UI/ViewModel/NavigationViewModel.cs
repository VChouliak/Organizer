using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models.Lookups;
using Organizer.Core.ViewModel;
using System;
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
            _eventAggregator.Subscribe<NavigationItemViewModel>(f =>
            {
                AfterFriendSaved(f);
            });
        }

        private void AfterFriendSaved(NavigationItemViewModel friend)
        {
            var lookupItem = Friends.Single(f => f.Id == friend.Id);
            lookupItem.DisplayMember = friend.DisplayMember;
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

    }
}
