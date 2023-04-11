using MySqlX.XDevAPI.Common;
using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Services;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models.Entities;
using Organizer.Data.Specifications;
using Organizer.Infrastructure.Command;
using Organizer.UI.Service;
using Organizer.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMessageDialogService _messageDialogService;
        private readonly IMeetingAsyncDataService _meetingDataService;
        private MeetingWrapper _meeting;

        public MeetingDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IMeetingAsyncDataService meetingDataService) : base(eventAggregator)
        {
            _messageDialogService = messageDialogService;
            _meetingDataService = meetingDataService;
        }

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

        public override async Task LoadAsync(int? id)
        {
            var result = await _meetingDataService.GetAllAsync(new MeetingsIncludeFriendsSpecification(id));
            InitializeMeeting(result);
        }     

        protected override async void OnDeleteExecute(object obj)
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Dow you really wont to delete entry {Meeting.Title}", "Question");
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
            await _meetingDataService.SaveAllChangesAsync();
            HasChanges = _meetingDataService.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        private void InitializeMeeting(IEnumerable<Meeting> result)
        {
            if (result.Any())
            {
                var meeting = result.FirstOrDefault();
                Meeting = new MeetingWrapper(meeting);
            }
            else
            {
                Meeting = new MeetingWrapper(CreateNewMeeting());
            }

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
            };
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Meeting.Id == 0)
            {
                Meeting.Title = ""; //Used for validation Message in View...
            }
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
    }
}
