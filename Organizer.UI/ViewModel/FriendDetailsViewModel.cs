using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models.Entities;
using Organizer.Core.Models.Lookups;
using Organizer.Data.Specifications;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using Organizer.UI.Service;
using Organizer.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class FriendDetailsViewModel : ViewModelBase, IDetailsViewModel
    {
        private IFriendAsyncDataService _friendDataService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IAsyncLookupService<LookupItem> _propgrammingLanguageLookupService;
        private FriendWrapper _friend;
        private bool _hasChanges;
        private FriendPhoneNumberWrapper _selectedPhoneNumber;

        public FriendDetailsViewModel(IFriendAsyncDataService friendsDataService, IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IAsyncLookupService<LookupItem> propgrammingLanguageLookupService)
        {
            _friendDataService = friendsDataService;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _propgrammingLanguageLookupService = propgrammingLanguageLookupService;
            // _phoneNumbersService = phoneNumbersService;

            SaveCommand = new RelayCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new RelayCommand(OnDeleteFriendExecute);
            AddPhoneNumberCommand = new RelayCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new RelayCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<FriendPhoneNumberWrapper>();
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

        public async Task LoadAsync(int? id)
        {
            var result = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameIncludePhoneNumbers(id));
            InitializeFriend(result);
            InitializeFriendPhoneNumbers(result.FirstOrDefault().PhoneNumbers);
            await LoadProgrammingLanguagesLookupAsync();

        }

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

        private void InitializeFriend(IEnumerable<Friend> result)
        {
            if (result.Any())
            {
                var friend = result.FirstOrDefault();
                Friend = new FriendWrapper(friend);
            }
            else
            {
                Friend = new FriendWrapper(CreateNewEntry());
            }

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
                Friend.FirstName = "";
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
        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand AddPhoneNumberCommand { get; set; }
        public ICommand RemovePhoneNumberCommand { get; set; }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ObservableCollection<FriendPhoneNumberWrapper> PhoneNumbers { get; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        private Friend CreateNewEntry()
        {
            var friend = new Friend();
            _friendDataService.AddAsync(friend);
            return friend;
        }

        private async void OnSaveExecute(object parameter)
        {
            await _friendDataService.SaveAllChangesAsync();
            HasChanges = _friendDataService.HasChanges();
            _eventAggregator.Publish<OnSaveFriendEventArgs>(new OnSaveFriendEventArgs(Friend.Id, $"{Friend.FirstName} {Friend.LastName}"));
        }

        private bool OnSaveCanExecute(object parameter)
        {
            return Friend != null
                && !Friend.HasErrors
                && PhoneNumbers.All(p => !p.HasErrors)
                && HasChanges;
        }

        private async void OnDeleteFriendExecute(object obj)
        {
            var dialogResult = _messageDialogService.ShowOkCancelDialog($"Do you really wont to delete Entry {Friend.FirstName} {Friend.LastName}", "Question");
            if (dialogResult == MessageDialogResult.OK)
            {
                await _friendDataService.DeleteAsync(Friend.Model);
                await _friendDataService.SaveAllChangesAsync();
                _eventAggregator.Publish<OnDeleteFriendEventArgs>(new OnDeleteFriendEventArgs(Friend.Id));
            }

        }

    }
}
