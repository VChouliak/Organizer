using Org.BouncyCastle.Asn1.Cmp;
using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models;
using Organizer.Core.Specifications;
using Organizer.Infrastructure.Command;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class FriendDetailsViewModel : ViewModelBase, IDetailsViewModel
    {
        private IFriendAsyncDataService _friendDataService;
        private readonly IEventAggregator _eventAggregator;

        public FriendDetailsViewModel(IFriendAsyncDataService friendsDataService, IEventAggregator eventAggregator)
        {
            _friendDataService = friendsDataService;
            _eventAggregator = eventAggregator;

            _eventAggregator.Subscribe<int>(async (Id) =>
            {
                await LoadAsync(Id);
            });

            SaveCommand = new RelayCommand(OnSaveExecute, OnSaveCanExecute);
        }

        public async Task LoadAsync(int id)
        {
            var result = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameSpecification(id));
            if (result.Any())
            {
                Friend = result.FirstOrDefault();
            }
        }

        private void OnSaveExecute(object parameter)
        {
            _friendDataService.UpdateAsync(Friend);
            if(_friendDataService.SaveAllChangesAsync().Result > 0)
            {
                _eventAggregator.Publish<NavigationItemViewModel>(new NavigationItemViewModel(Friend.Id, $"{Friend.FirstName} {Friend.LastName}"));
            }
            
        }

        private bool OnSaveCanExecute(object parameter)
        {
            return true;
        }

        private Friend _friend;

        public Friend Friend
        {
            get { return _friend; }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand{ get; set; }

    }
}
