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
        private readonly Func<IDetailViewModel> _detailViewModelCreator;
        private readonly Func<IMeetingDetailViewModel> _meettingDetailsViewModelCreator;
        private IDetailViewModel _detailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel, Func<FriendDetailViewModel> detailsViewModelCreator, Func<MeetingDetailViewModel> meettingDetailsViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _detailViewModelCreator = detailsViewModelCreator;
            _meettingDetailsViewModelCreator = meettingDetailsViewModelCreator;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _eventAggregator.Subscribe<OpenDetailViewEventArgs>(async detailViewModelArgs =>
            {
                await OnOpenDetailViewAsync(detailViewModelArgs);
            });

            _eventAggregator.Subscribe<AfterDetailDeletedEventArgs>(eventargs => AfterEntryDeleted(eventargs));

            NavigationViewModel = navigationViewModel;
            CreateNewDetailCommand = new RelayCommand(OnCreateNewDetailExecute);
        }

        private void AfterEntryDeleted(AfterDetailDeletedEventArgs args)
        {
            DetailViewModel = null;
        }

        public ICommand CreateNewDetailCommand { get; set; }

        public INavigationViewModel NavigationViewModel { get; }

        public IDetailViewModel DetailViewModel
        {
            get { return _detailViewModel; }
            private set
            {
                _detailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public async Task OnOpenDetailViewAsync(OpenDetailViewEventArgs args)
        {
            if (DetailViewModel != null && DetailViewModel.HasChanges)
            {
                var messageBoxResult = _messageDialogService.ShowOkCancelDialog("You have made changes. Navigate away ?", "Question");
                if (messageBoxResult == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            switch (args.ViewModel)
            {
                case nameof(FriendDetailViewModel):
                    DetailViewModel = _detailViewModelCreator();
                    break;
                case nameof(MeetingDetailViewModel):
                    DetailViewModel = _meettingDetailsViewModelCreator();
                    break;
                default:
                    throw new Exception($"ViewModel {args.ViewModel} not mapped");
            }
            await DetailViewModel.LoadAsync(args.Id);
        }

        private async void OnCreateNewDetailExecute(object viewModelType)
        {
            await OnOpenDetailViewAsync(new OpenDetailViewEventArgs {ViewModel = ((Type)viewModelType).Name});
        }
    }
}
