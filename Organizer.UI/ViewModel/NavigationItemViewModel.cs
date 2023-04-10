using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Infrastructure.Command;
using Organizer.UI.Events;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayMember;
        private readonly string _detailViewModelName;

        public NavigationItemViewModel(int id, string displayMember, string detailViewModelName, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            _detailViewModelName = detailViewModelName;
            _eventAggregator = eventAggregator;
            OpenDetailViewCommand = new RelayCommand(OnOpenDetailViewExecute);
        }       

        public int Id { get; }

        public string DisplayMember
        {
            get { return _displayMember; }
            set
            {
                _displayMember = value;
                OnPropertyChanged();
            }
        }


        public ICommand OpenDetailViewCommand { get; }

        private void OnOpenDetailViewExecute(object obj)
        {
            _eventAggregator.Publish<OpenDetailViewEventArgs>( new () { Id = Id, ViewModel = _detailViewModelName});
        }
    }
}
