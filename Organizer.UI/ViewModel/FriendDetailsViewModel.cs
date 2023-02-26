using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Data.Specifications;
using Organizer.Infrastructure.Command;
using Organizer.UI.Wrapper;
using System.Linq;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class FriendDetailsViewModel : ViewModelBase, IDetailsViewModel
    {
        private IFriendAsyncDataService _friendDataService;
        private readonly IEventAggregator _eventAggregator;
        private FriendWrapper _friend;
        private bool _hasChanges;

        public FriendDetailsViewModel(IFriendAsyncDataService friendsDataService, IEventAggregator eventAggregator)
        {
            _friendDataService = friendsDataService;
            _eventAggregator = eventAggregator;

            SaveCommand = new RelayCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int id)
        {
            var result = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameSpecification(id));
            if (result.Any())
            {
                var friend = result.FirstOrDefault();
                Friend = new FriendWrapper(friend);
            }

            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendDataService.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    SaveCommand.RaiseCanExecuteChanged();
                }
            };
            SaveCommand.RaiseCanExecuteChanged();
        }

        public FriendWrapper Friend
        {
            get { return _friend; }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; set; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    SaveCommand.RaiseCanExecuteChanged();
                }
            }
        }


        private async void OnSaveExecute(object parameter)
        {
            await _friendDataService.UpdateAsync(Friend.Model);
            var saveResult = await _friendDataService.SaveAllChangesAsync();

            if (saveResult > 0)
            {
                HasChanges = _friendDataService.HasChanges();
                _eventAggregator.Publish<NavigationItemViewModel>(new NavigationItemViewModel(Friend.Id, $"{Friend.FirstName} {Friend.LastName}", _eventAggregator));
            }

        }

        private bool OnSaveCanExecute(object parameter)
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

    }
}
