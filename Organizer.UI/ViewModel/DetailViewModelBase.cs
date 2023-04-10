using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public abstract class DetailViewModelBase : ViewModelBase, IDetailViewModel
    {
        private bool _hasChanges;
        private readonly IEventAggregator _eventAggregator;

        protected DetailViewModelBase(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            SaveCommand = new RelayCommand(OnSaveExecute,OnSaveCanExecute);
            DeleteCommand = new RelayCommand(OnDeleteExecute);
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

        public abstract Task LoadAsync(int? id);
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        protected abstract void OnDeleteExecute(object obj);
        protected abstract bool OnSaveCanExecute(object obj);
        protected abstract void OnSaveExecute(object obj);

        protected virtual void RaiseDetailDeletedEvent(int modelId)
        {
            _eventAggregator.Publish<AfterDetailDeletedEventArgs>(new()
            {
                Id = modelId,
                ViewModel = this.GetType().Name
            });
        }

        protected virtual void RaiseDetailSavedEvent(int modelId, string displayMember)
        {
            _eventAggregator.Publish<AfterDetailSavedEventArgs>(new ()
            {
                Id = modelId,
                DisplayMember = displayMember,
                ViewModel = this.GetType().Name
            });
        }
    }
}

