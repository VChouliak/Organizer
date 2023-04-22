using Organizer.Core.Interfaces.Events.Aggregator;
using Organizer.Core.Interfaces.Services;
using Organizer.Core.Interfaces.ViewModels;
using Organizer.Core.Models.Entities;
using Organizer.Infrastructure.Command;
using Organizer.UI.Service;
using Organizer.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class ProgrammingLanguageDetailViewModel : DetailViewModelBase, IDetailViewModel
    {
        private readonly IProgrammingLanguageAsyncDataService _dataService;
        private ProgrammingLanguageWrapper _selectedProgrammingLanguage;

        public ProgrammingLanguageDetailViewModel(IEventAggregator eventAggregator, IMessageDialogService messageDialogService, IProgrammingLanguageAsyncDataService dataService) : base(eventAggregator, messageDialogService)
        {
            Title = "Programming Languages";
            _dataService = dataService;
            ProgrammingLanguages = new ObservableCollection<ProgrammingLanguageWrapper>();

            AddCommand = new RelayCommand(OnAddExecute);
            RemoveCommand = new RelayCommand(OnDeleteExecute, OnDeleteCanExecut);
        }
        
        public ProgrammingLanguageWrapper SelectedProgrammingLanguage
        {
            get { return _selectedProgrammingLanguage; }
            set
            {
                _selectedProgrammingLanguage = value;
                OnPropertyChanged();
                ((RelayCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }
        public ObservableCollection<ProgrammingLanguageWrapper> ProgrammingLanguages { get; }
       
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }

        public override async Task LoadAsync(int id)
        {
            Id = id;
            foreach (var wrapper in ProgrammingLanguages)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }
            ProgrammingLanguages.Clear();
            var languages = await _dataService.GetAllAsync();

            foreach (var model in languages)
            {
                var wrapper = new ProgrammingLanguageWrapper(model); ;
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ProgrammingLanguages.Add(wrapper);
            }
        }

        protected async override void OnDeleteExecute(object obj)
        {
            var isReferenced = await _dataService.IsReferencedByFriendAsync(SelectedProgrammingLanguage.Id);
            if (isReferenced)
            {
                MessageDialogService.ShowInfoDialog($"The language {SelectedProgrammingLanguage.Name} can't be removed, as it is referenced by at least one friend");
                return;
            }

            SelectedProgrammingLanguage.PropertyChanged -= Wrapper_PropertyChanged;
            await _dataService.DeleteAsync(SelectedProgrammingLanguage.Model);
            ProgrammingLanguages.Remove(SelectedProgrammingLanguage);
            SelectedProgrammingLanguage = null;
            HasChanges = _dataService.HasChanges();
            ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        protected override bool OnSaveCanExecute(object obj)
        {
            return HasChanges && ProgrammingLanguages.All(p => !p.HasErrors);
        }

        protected override async void OnSaveExecute(object obj)
        {
            try
            {
                await _dataService.SaveAllChangesAsync();
                HasChanges = _dataService.HasChanges();
                RaiseCollectionSavedEvent();
            }
            catch (Exception ex)
            {

                while(ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageDialogService.ShowInfoDialog("Error while saving the entities the data will be reloaded. Details: " + ex.Message);
                await LoadAsync(Id);
            }
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _dataService.HasChanges();
            }
            if (e.PropertyName == nameof(ProgrammingLanguageWrapper.HasErrors))
            {
                ((RelayCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private bool OnDeleteCanExecut(object arg)
        {
            return SelectedProgrammingLanguage != null;
        }

        private void OnAddExecute(object obj)
        {
            var wrapper = new ProgrammingLanguageWrapper(new ProgrammingLanguage());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            _dataService.AddAsync(wrapper.Model);
            ProgrammingLanguages.Add(wrapper);
            wrapper.Name = ""; //Trigger the validation
        }
    }
}
