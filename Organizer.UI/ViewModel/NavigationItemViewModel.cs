using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Infrastructure.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private string _displayMember;

        public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
        {
            Id = id;
            DisplayMember = displayMember;
            _eventAggregator = eventAggregator;
            OpenFriendDetailViewCommand = new RelayCommand(OnOpenFriendDetailView, OnCanOpenFriendDetailView);
        }

        private bool OnCanOpenFriendDetailView(object arg)
        {
            return true; // TODO: implement
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
        public ICommand OpenFriendDetailViewCommand { get; }

        private void OnOpenFriendDetailView(object obj)
        {
            _eventAggregator.Publish<int>(Id);
        }
    }
}
