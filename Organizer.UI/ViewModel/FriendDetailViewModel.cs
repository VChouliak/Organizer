using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models.Entities;
using Organizer.Core.Models.Lookups;
using Organizer.Data.Specifications;
using Organizer.Infrastructure.Command;
using Organizer.UI.Service;
using Organizer.UI.Wrapper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class FriendDetailViewModel : DetailViewModelBase, IDetailViewModel
    {
        private IFriendsAsyncDataService _friendDataService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IAsyncLookupService<LookupItem> _propgrammingLanguageLookupService;
        private FriendWrapper _friend;
        private bool _hasChanges;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;

        public FriendDetailViewModel(
            IFriendsAsyncDataService friendsDataService,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IAsyncLookupService<LookupItem> propgrammingLanguageLookupService)
            : base(eventAggregator)
        {
            _friendDataService = friendsDataService;
            _messageDialogService = messageDialogService;
            _propgrammingLanguageLookupService = propgrammingLanguageLookupService;

            AddPhoneNumberCommand = new RelayCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new RelayCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
        }

        public override async Task LoadAsync(int? id)
        {
            var result = await _friendDataService.GetEntityWithSpecificationAsync(new FriendsOrderedByFirstNameIncludePhoneNumbersAndMeetingsSpecification(id));
            InitializeFriend(result);
            if (result != null)
            {
                InitializeFriendPhoneNumbers(result.PhoneNumbers);
            }
            await LoadProgrammingLanguagesLookupAsync();

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
        public FriendPhoneNumberWrapper SelectedPhoneNumber
        {
            get
            {
                return _selectedPhoneNumber;
            }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((RelayCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand AddPhoneNumberCommand { get; set; }
        public ICommand RemovePhoneNumberCommand { get; set; }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

        private void InitializeFriendPhoneNumbers(ICollection<FriendPhoneNumber> phoneNumbers)
        {
            foreach (var wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (var friendPhoneNumber in phoneNumbers)
            {
                var wrapper = new FriendPhoneNumberWrapper(friendPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void FriendPhoneNumberWrapper_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _friendDataService.HasChanges();
            }
            if (e.PropertyName == nameof(FriendPhoneNumberWrapper.HasErrors))
            {
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = friend != null ? new FriendWrapper(friend) : new FriendWrapper(CreateNewEntry());

            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendDataService.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                Friend.FirstName = ""; //For Validation message in view...
                Friend.LastName = "";
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            var lookup = await _propgrammingLanguageLookupService.GetLookupAsync();

            foreach (var lookupitem in lookup)
            {
                ProgrammingLanguages.Add(lookupitem);
            }
        }

        private Friend CreateNewEntry()
        {
            var friend = new Friend();
            _friendDataService.AddAsync(friend);
            return friend;
        }

        private void OnRemovePhoneNumberExecute(object obj)
        {
            SelectedPhoneNumber.PropertyChanged -= FriendPhoneNumberWrapper_PropertyChanged;
            Friend.Model.PhoneNumbers.Remove(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _friendDataService.HasChanges();
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddPhoneNumberExecute(object obj)
        {
            var newNumber = new FriendPhoneNumberWrapper(new FriendPhoneNumber());
            newNumber.PropertyChanged += FriendPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Friend.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = ""; //For Validation Message...
        }

        private bool OnRemovePhoneNumberCanExecute(object obj)
        {
            return _selectedPhoneNumber != null;
        }
        protected override async void OnSaveExecute(object parameter)
        {
            await _friendDataService.SaveAllChangesAsync();
            HasChanges = _friendDataService.HasChanges();
            RaiseDetailSavedEvent(Friend.Id, $"{Friend.FirstName} {Friend.LastName}");
        }

        protected override bool OnSaveCanExecute(object parameter)
        {
            return Friend != null
                && !Friend.HasErrors
                && PhoneNumbers.All(p => !p.HasErrors)
                && HasChanges;
        }

        protected override async void OnDeleteExecute(object obj)
        {
            if (Friend.Model.Meetings.Any())
            {
                _messageDialogService.ShowInfoDialog($"{Friend.FirstName} {Friend.LastName} can not be deleted, at this friend is part of at least one meeting");
                return;
            }

            var dialogResult = _messageDialogService.ShowOkCancelDialog($"Do you really wont to delete Entry {Friend.FirstName} {Friend.LastName}", "Question");
            if (dialogResult == MessageDialogResult.OK)
            {
                await _friendDataService.DeleteAsync(Friend.Model);
                await _friendDataService.SaveAllChangesAsync();
                RaiseDetailDeletedEvent(Friend.Id);
            }

        }

    }
}
