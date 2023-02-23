using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.ViewModel;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel, IDetailsViewModel detailsViewModel)
        {
            NavigationViewModel = navigationViewModel;
            DetailsViewModel = detailsViewModel;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }
        public IDetailsViewModel DetailsViewModel { get; }
    }
}
