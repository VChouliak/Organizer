using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models;
using Organizer.Core.ViewModel;
using Organizer.UI.Event;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private IAsyncLookupService<LookupItem> _lookupService;
        private LookupItem _selectedFriend;

        public NavigationViewModel(IAsyncLookupService<LookupItem> lookupService)
        {
            _lookupService = lookupService;
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
                {
                    EventsMediator.Instance.OnPropertyChanged(this, new LookupViewEventArgs(this.SelectedFriend.Id));
                }
            }
        }      

    }
}
