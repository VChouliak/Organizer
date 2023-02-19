using Organizer.Core.Interfaces.Service;
using Organizer.Core.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFriendAsyncDataService _friendDataService;
        private Friend _selectedFriend;      

        public MainViewModel(IFriendAsyncDataService friendDataService)
        {
            Friends = new ObservableCollection<Friend>();
            _friendDataService = friendDataService;
        }

        public async Task LoadAsync()
        {
            Friends.Clear();
            var friends = await _friendDataService.GetAllAsync();
            
            foreach ( var friend in friends)
            {
                Friends.Add( friend );

            }
                
                
        }
        public ObservableCollection<Friend> Friends { get; set; }
        public Friend SelectedFriend
        {
            get { return _selectedFriend; }
            set
            {
                _selectedFriend = value;
                OnPropertyChanged();
            }
        }

    }
}
