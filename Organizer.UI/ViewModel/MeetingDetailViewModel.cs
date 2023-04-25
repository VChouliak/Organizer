using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.Services;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models.Entities;
using Organizer.Data.Specifications;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using Organizer.UI.Service;
using Organizer.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingAsyncDataService _meetingDataService;
        private readonly IFriendsAsyncDataService _friendDataService;
        private MeetingWrapper _meeting;
        private Friend _selectedAvailableFriend;
        private Friend _selectedAddedFriend;

        private IEnumerable<Friend> _allFriends;

        public MeetingDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IMeetingAsyncDataService meetingDataService, IFriendsAsyncDataService friendDataService) : base(eventAggregator, messageDialogService)
        {
            _meetingDataService = meetingDataService;
            _friendDataService = friendDataService;

            AddedFriends = new ObservableCollection<Friend>();
            AvailableFriends = new ObservableCollection<Friend>();

            AddFriendCommand = new RelayCommand(OnAddFriendExecute, OnAddFiendCanExecute);
            RemoveFriendCommand = new RelayCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);

            eventAggregator.Subscribe<AfterDetailSavedEventArgs>(AfterDetailSaved);
            eventAggregator.Subscribe<AfterDetailDeletedEventArgs>(AfterDetailDeleted);
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs eventargs)
        {
            if (eventargs.ViewModelName == nameof(FriendDetailViewModel))
            {               
                _allFriends = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameIncludePhoneNumbersAndMeetingsSpecification());
                SetupPicklist();
            }
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs eventargs)
        {
            if (eventargs.ViewModelName == nameof(FriendDetailViewModel))
            {
                await _friendDataService.ReloadFriendAsync(eventargs.Id);        
                _allFriends = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameIncludePhoneNumbersAndMeetingsSpecification());
                SetupPicklist();
            }
        }

        public ObservableCollection<Friend> AddedFriends { get; }
        public ObservableCollection<Friend> AvailableFriends { get; }

        public ICommand AddFriendCommand { get; }
        public ICommand RemoveFriendCommand { get; }

        public MeetingWrapper Meeting
        {
            get
            {
                return _meeting;
            }
            set
            {
                _meeting = value;
                OnPropertyChanged();
            }
        }
        public Friend SelectedAvailableFriend
        {
            get => _selectedAvailableFriend;
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((RelayCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }
        public Friend SelectedAddedFriend
        {
            get => _selectedAddedFriend;
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((RelayCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int id)
        {
            var meeting = id > 0 ? await _meetingDataService.GetEntityWithSpecificationAsync(new MeetingsIncludeFriendsSpecification(id)) : CreateNewMeeting();

            Id = id;

            InitializeMeeting(meeting);

            _allFriends = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameIncludePhoneNumbersAndMeetingsSpecification());
            SetupPicklist();
        }

        protected override async void OnDeleteExecute(object obj)
        {
            var result = MessageDialogService.ShowOkCancelDialog($"Dow you really wont to delete entry {Meeting.Title}", "Question");
            if (result == MessageDialogResult.OK)
            {
                await _meetingDataService.DeleteAsync(Meeting.Model);
                await _meetingDataService.SaveAllChangesAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute(object obj)
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute(object obj)
        {
            await SaveWithOptimisticConcurrencyAsync(_meetingDataService.SaveAllChangesAsync,
                () =>
                {
                    HasChanges = _friendDataService.HasChanges();
                    Id = Meeting.Id;
                    RaiseDetailSavedEvent(Meeting.Id, $"²{Meeting.Title}");
                });           
        }

        private void SetupPicklist()
        {
            var meetingFriendsIds = Meeting.Model.Friends.Select(f => f.Id);
            var addedFriends = _allFriends.Where(f => meetingFriendsIds.Contains(f.Id));
            var availableFriends = _allFriends.Except(addedFriends);

            AddedFriends.Clear();
            AvailableFriends.Clear();

            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }
            foreach (var availableFriend in availableFriends)
            {
                AvailableFriends.Add(availableFriend);
            }
        }
        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = meeting != null ? new MeetingWrapper(meeting) : new MeetingWrapper(CreateNewMeeting());

            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingDataService.HasChanges();
                }
                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Meeting.Title))
                {
                    SetTitle();
                }
            };
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Meeting.Id == 0)
            {
                Meeting.Title = ""; //Used for validation Message in View...
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }

        private Meeting CreateNewMeeting()
        {
            var newMeeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date,
            };
            _meetingDataService.AddAsync(newMeeting);
            return newMeeting;
        }

        private bool OnRemoveFriendCanExecute(object arg)
        {
            return SelectedAddedFriend != null;
        }

        private void OnRemoveFriendExecute(object obj)
        {
            var friendToRemove = SelectedAddedFriend;

            Meeting.Model.Friends.Remove(friendToRemove);
            AddedFriends.Remove(friendToRemove);
            AvailableFriends.Add(friendToRemove);
            HasChanges = _meetingDataService.HasChanges();
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddFiendCanExecute(object arg)
        {
            return SelectedAvailableFriend != null;
        }

        private void OnAddFriendExecute(object obj)
        {
            var friendToAdd = SelectedAvailableFriend;

            Meeting.Model.Friends.Add(friendToAdd);
            AddedFriends.Add(friendToAdd);
            AvailableFriends.Remove(friendToAdd);
            HasChanges = _meetingDataService.HasChanges();
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }
    }
}
