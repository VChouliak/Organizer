using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models;
using Organizer.Core.ViewModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IAsyncLookupService<LookupItem> _lookupService;
        private readonly IEventAggregator _eventAggregator;
        private LookupItem _selectedFriend;

        public NavigationViewModel(IAsyncLookupService<LookupItem> lookupService, IEventAggregator eventAggregator)
        {
            _lookupService = lookupService;
            _eventAggregator = eventAggregator;
            Friends = new ObservableCollection<LookupItem>();
        }

        public async Task LoadAsync()
        {
            var lookup = await _lookupService.GetLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(item);
            }

        }

        public ObservableCollection<LookupItem> Friends { get; }
        public LookupItem SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
                if(_selectedFriend != null)
                {                    ;
                    _eventAggregator.Publish<int>(this.SelectedFriend.Id);
                }
            }
        }      

    }
}
