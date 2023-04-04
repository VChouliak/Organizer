using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.ViewModel;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using Organizer.UI.Service;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

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
                await LoadFriendDetailsViewAsync(Id);
            });

            _eventAggregator.Subscribe<OnDeleteFriendEventArgs>(i => AfterEntryDeleted());

            NavigationViewModel = navigationViewModel;
            CreateNewEntryCommand = new RelayCommand(OnCreateNewEntry, OnCanCreateNewEntry);
        }

        private void AfterEntryDeleted()
        {
            FriendDetailsViewModel = null;
        }

        public ICommand CreateNewEntryCommand { get; set; }

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

        public async Task LoadFriendDetailsViewAsync(int? id)
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

        private bool OnCanCreateNewEntry(object arg)
        {
            return true; //TODO: Adjust
        }

        private async void OnCreateNewEntry(object obj)
        {
           await LoadFriendDetailsViewAsync(null);
        }
    }
}
