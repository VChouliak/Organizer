using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.ViewModel;
using Organizer.UI.Service;
using System;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly Func<IDetailsViewModel> _detailsViewModelCreator;
        private IDetailsViewModel _friendDetailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel, Func<IDetailsViewModel> detailsViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _detailsViewModelCreator = detailsViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.Subscribe<int>(async (Id) =>
            {
                LoadFriendDetailsViewAsync(Id);
            });

            NavigationViewModel = navigationViewModel;
        }

        public INavigationViewModel NavigationViewModel { get; }

        public IDetailsViewModel FriendDetailsViewModel
        {
            get { return _friendDetailViewModel; }
            private set
            {
                _friendDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public async void LoadFriendDetailsViewAsync(int id)
        {
            if (FriendDetailsViewModel != null && FriendDetailsViewModel.HasChanges)
            {
                var messageBoxResult = _messageDialogService.ShowOkCancelDialog("You have made changes. Navigate away ?", "Question");
                if (messageBoxResult == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            FriendDetailsViewModel = _detailsViewModelCreator();
            await FriendDetailsViewModel.LoadAsync(id);
        }
    }
}
