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
        private IFriendAsyncDataService _dataService;

        public FriendDetailsViewModel(IFriendAsyncDataService dataService)
        {
            _dataService = dataService;
        }     

        public async Task LoadAsync(int id)
        {
            var result = await _dataService.GetAllAsync(new FriendsOrderedByFirstNameSpecification(id));
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
