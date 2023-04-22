using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using Organizer.UI.Service;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        protected bool _hasChanges;
        protected readonly IEventAggregator EventAggregator;
        protected readonly IMessageDialogService MessageDialogService;
        private int _id;
        protected string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            protected set
            {
                _title = value;
                OnPropertyChanged();
            }
        }


        protected DetailViewModelBase(IEventAggregator eventAggregator, IMessageDialogService messageDialogService)
        {
            EventAggregator = eventAggregator;
            MessageDialogService = messageDialogService;
            SaveCommand = new RelayCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new RelayCommand(OnDeleteExecute);
            CloseDetailViewCommand = new RelayCommand(OnCloseDetailViewExecute);
        }
       
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

        public abstract Task LoadAsync(int id);
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CloseDetailViewCommand { get; private set; }
        public int Id
        {
            get { return _id; }
            protected set { _id = value; }
        }

        protected virtual void OnCloseDetailViewExecute(object obj)
        {
            if (HasChanges)
            {
                var result = MessageDialogService.ShowOkCancelDialog("Yout've made changes. Close this item ?", "Question");

                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }

            EventAggregator.Publish<AfterDetailClosedEventArgs>(new() { Id = this.Id, ViewModelName = this.GetType().Name });
        }
        protected abstract void OnDeleteExecute(object obj);
        protected abstract bool OnSaveCanExecute(object obj);
        protected abstract void OnSaveExecute(object obj);

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            EventAggregator.Publish<AfterDetailDeletedEventArgs>(new()
            {
                Id = modelId,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            EventAggregator.Publish<AfterDetailSavedEventArgs>(new()
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModelName = this.GetType().Name
            });
        }

        protected virtual void RaiseCollectionSavedEvent()
        {
            EventAggregator.Publish<AfterCollectionSavedEventArgs>(new() { VieModelName = this.GetType().Name});
        }
    }
}

