using Organizer.Core.Interfaces;
using Organizer.Core.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Organizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFriendDataService _friendDataService;
        private Friend _selectedFriend;      

        public MainViewModel(IFriendDataService friendDataService)
        {
            Friends = new ObservableCollection<Friend>();
            _friendDataService = friendDataService;
        }

        public void Load()
        {
            Friends.Clear();
            _friendDataService
                .GetAll()
                .ToList()
                .ForEach(friend => Friends.Add(friend));
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
