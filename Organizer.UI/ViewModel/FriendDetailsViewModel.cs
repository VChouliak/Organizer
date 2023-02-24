using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models;
using Organizer.Core.Specifications;
using Organizer.UI.Event;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class FriendDetailsViewModel : ViewModelBase, IDetailsViewModel
    {
        private IFriendAsyncDataService _friendDataService;

        public FriendDetailsViewModel(IFriendAsyncDataService friendsDataService)
        {
            _friendDataService = friendsDataService;
            EventsMediator.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object? sender, LookupViewEventArgs e)
        {
            var test = e;          
            OnOpenFriendDetailView(e.Id);

           
        }

        private async void OnOpenFriendDetailView(int id)
        {
            await LoadAsync(id);
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
