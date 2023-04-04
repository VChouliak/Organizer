﻿using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Service;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models.Entities;
using Organizer.Data.Specifications;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using Organizer.UI.Service;
using Organizer.UI.Wrapper;
using System;
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
        private FriendWrapper _friend;
        private bool _hasChanges;

        public FriendDetailsViewModel(IFriendAsyncDataService friendsDataService, IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            _friendDataService = friendsDataService;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new RelayCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new RelayCommand(OnDeleteExecute, OnDeleteCanExecute);
        }

        public async Task LoadAsync(int? id)
        {
            var result = await _friendDataService.GetAllAsync(new FriendsOrderedByFirstNameSpecification(id));
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
                    SaveCommand.RaiseCanExecuteChanged();
                }
            };
            SaveCommand.RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                Friend.FirstName = "";
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

        public RelayCommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    SaveCommand.RaiseCanExecuteChanged();
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
            return Friend != null && !Friend.HasErrors && HasChanges;
        }

        private bool OnDeleteCanExecute(object arg)
        {
            return true;
        }

        private async void OnDeleteExecute(object obj)
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
