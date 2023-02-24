using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models;
using Organizer.Core.Specifications;
using System.Linq;
using System.Threading.Tasks;

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

        }

        public async Task LoadAsync(int id)
        {
            var result = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameSpecification(id));
            if (result.Any())
            {
                Friend = result.FirstOrDefault();
            }
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

    }
}
