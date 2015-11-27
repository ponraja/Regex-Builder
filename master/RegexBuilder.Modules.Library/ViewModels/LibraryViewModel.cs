using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using RegexBuilder.Infrastructure.Events;
using RegexBuilder.Infrastructure.Models;
using RegexBuilder.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace RegexBuilder.Modules.Library.ViewModels
{
    [Export]
    public class LibraryViewModel : BindableBase, INavigationAware
    {
        private LibraryRepository libraryRepository;
        private ObservableCollection<RegularExpression> _regularExpression;
        private CompositionContainer Container;
        private IRegionManager regionManager;
        private IRegionNavigationJournal navigationJournal;
        private RegularExpression _selectedRegularExpression;
        private IEventAggregator eventAggregator;


        public ObservableCollection<RegularExpression> RegularExpressions
        {
            get { return _regularExpression; }
            set { SetProperty(ref _regularExpression, value); }
        }

        public RegularExpression SelectedRegularExpression
        {
            get { return _selectedRegularExpression; }
            set
            {
                SetProperty(ref _selectedRegularExpression, value);
                SelectedRegularExpressionCommand.RaiseCanExecuteChanged();
            }
        }

        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }

        #region Commands

        public ICommand AddRegularExpressionCommand { get; private set; }
        public ICommand EditRegularExpressionCommand { get; private set; }
        public ICommand DeleteRegularExpressionCommand { get; private set; }
        public ICommand CopyRegularExpressionCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public DelegateCommand SelectedRegularExpressionCommand { get; private set; }

        #endregion

        [ImportingConstructor]
        public LibraryViewModel(CompositionContainer container, IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            this.Container = container;
            this.regionManager = regionManager;
            this.eventAggregator = eventAggregator;
            libraryRepository = container.GetExportedValue<LibraryRepository>();
            RegularExpressions = libraryRepository.RegularExpressions;
            this.eventAggregator.GetEvent<UpdateRegularExpressionCollectionEvent>().Subscribe(UpdateRegularExpressionCollectionEventHandler);
            this.ConfirmationRequest = new InteractionRequest<IConfirmation>();
            AddRegularExpressionCommand = new DelegateCommand(AddRegularExpressionCommandHandler);
            EditRegularExpressionCommand = new DelegateCommand<RegularExpression>(EditRegularExpressionCommandHandler);
            DeleteRegularExpressionCommand = new DelegateCommand<RegularExpression>(DeleteRegularExpressionCommandHandler);
            CopyRegularExpressionCommand = new DelegateCommand<RegularExpression>(CopyRegularExpressionCommandHandler);
            SelectedRegularExpressionCommand = new DelegateCommand(SelectedRegularExpressionCommandHandler, CanExecuteSelectedRegularExpressionCommand);
            CancelCommand = new DelegateCommand(CancelCommandHandler);
        }

        private void CopyRegularExpressionCommandHandler(RegularExpression exp)
        {
            if(exp != null)
            {
                Clipboard.SetDataObject(exp.Expression, true);
            }
        }

        private void UpdateRegularExpressionCollectionEventHandler(NotifyCollectionChangedAction obj)
        {
            RegularExpressions = libraryRepository.RegularExpressions;
        }

        private void DeleteRegularExpressionCommandHandler(RegularExpression exp)
        {
            if (exp != null)
            {
                this.ConfirmationRequest.Raise(new Confirmation { Content = string.Format("Did you want to delete {0} Regex?", exp.Title), Title = "Delete confirmation" },
                    c =>
                    {
                        if (c.Confirmed)
                            libraryRepository.DeleteRegularExpression(exp);
                    });
            }
        }

        private void SelectedRegularExpressionCommandHandler()
        {
            eventAggregator.GetEvent<SelectedRegularExpressionEvent>().Publish(SelectedRegularExpression);
            navigationJournal.GoBack();
        }

        private bool CanExecuteSelectedRegularExpressionCommand()
        {
            return SelectedRegularExpression != null;
        }

        private void AddRegularExpressionCommandHandler()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Add", string.Empty);
            this.regionManager.RequestNavigate("MainRegion", new Uri("ManipulationView" + parameters, UriKind.Relative));
        }

        private void CancelCommandHandler()
        {
            this.navigationJournal.GoBack();
        }

        private void EditRegularExpressionCommandHandler(RegularExpression regex)
        {
            if (regex != null)
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("Edit", regex.Id);
                this.regionManager.RequestNavigate("MainRegion", new Uri("ManipulationView" + parameters, UriKind.Relative));
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedRegularExpression = null;
            this.navigationJournal = navigationContext.NavigationService.Journal;
        }

        
    }
}
