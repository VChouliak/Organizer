using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.ViewModel;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using Organizer.UI.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDictionary<string, Func<IDetailViewModel>> _detailViewModelCreator;
        private IDetailViewModel _selectedDetailViewModel;

        public MainViewModel(INavigationViewModel navigationViewModel, Dictionary<string, Func<IDetailViewModel>> detailsViewModelCreator, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _detailViewModelCreator = detailsViewModelCreator;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<OpenDetailViewEventArgs>(async detailViewModelArgs =>
            {
                await OnOpenDetailViewAsync(detailViewModelArgs);
            });

            _eventAggregator.Subscribe<AfterDetailDeletedEventArgs>(eventargs => AfterDetailDeletedExecute(eventargs));
            _eventAggregator.Subscribe<AfterDetailClosedEventArgs>(eventargs => AfterDetailClosed(eventargs));

            NavigationViewModel = navigationViewModel;
            CreateNewDetailCommand = new RelayCommand(OnCreateNewDetailExecute);

            DetailViewModels = new ObservableCollection<IDetailViewModel>();
        }

        public ICommand CreateNewDetailCommand { get; }

        public INavigationViewModel NavigationViewModel { get; }
        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }
        public IDetailViewModel SelectedDetailViewModel
        {
            get
            {
                return _selectedDetailViewModel;
            }
            set
            {
                _selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


        private void AfterDetailDeletedExecute(AfterDetailDeletedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModel);
        }


        private void AfterDetailClosed(AfterDetailClosedEventArgs eventargs)
        {
            RemoveDetailViewModel(eventargs.Id, eventargs.ViewModelName);
        }

        private void RemoveDetailViewModel(int? id, string viewModelName)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == id && vm.GetType().Name == viewModelName);
            if (detailViewModel != null)
            {
                DetailViewModels.Remove(detailViewModel);
            }
        }

        public async Task OnOpenDetailViewAsync(OpenDetailViewEventArgs args)
        {
            var detailViewModel = DetailViewModels.SingleOrDefault(vm => vm.Id == args.Id && vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = _detailViewModelCreator[args.ViewModelName]();
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }

            SelectedDetailViewModel = detailViewModel;
        }

        private async void OnCreateNewDetailExecute(object viewModelType)
        {
            await OnOpenDetailViewAsync(new OpenDetailViewEventArgs { ViewModelName = ((Type)viewModelType).Name });
        }

    }
}
