using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using RegexBuilder.Infrastructure.Models;
using RegexBuilder.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RegexBuilder.Modules.Library.ViewModels
{
    [Export]
    public class ManipulationViewModel : BindableBase, IConfirmNavigationRequest
    {

        private LibraryRepository libraryRepository;
        private CompositionContainer Container;
        private IRegionNavigationJournal navigationJournal;

        private RegularExpression regularExpression;
        public RegularExpression RegularExpression
        {
            get { return regularExpression; }
            set { SetProperty(ref regularExpression, value); }
        }

        private bool isAddingNewItem;
        public bool IsAddingNewItem
        {
            get { return isAddingNewItem; }
            set { SetProperty(ref isAddingNewItem, value); }
        }

        #region Commands

        public ICommand AddRegularExpressionCommand { get; private set; }
        public ICommand UpdateRegularExpressionCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        #endregion

        [ImportingConstructor]
        public ManipulationViewModel(CompositionContainer container)
        {
            this.Container = container;
            libraryRepository = this.Container.GetExportedValue<LibraryRepository>();

            AddRegularExpressionCommand = new DelegateCommand(AddRegularExpressionCommandHandler);
            UpdateRegularExpressionCommand = new DelegateCommand(UpdateRegularExpressionCommandHandler);
            CancelCommand = new DelegateCommand(CancelCommandHandler);
        }

        private void AddRegularExpressionCommandHandler()
        {
            libraryRepository.AddRegularExpression(RegularExpression);
            this.navigationJournal.GoBack();
        }

        private void CancelCommandHandler()
        {
            this.navigationJournal.GoBack();
        }

        private void UpdateRegularExpressionCommandHandler()
        {
            libraryRepository.UpdateRegularExpression(RegularExpression);
            this.navigationJournal.GoBack();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //navigationContext.Parameters.Add("test", "test");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var id = navigationContext.Parameters["Edit"];
            if (id != null)
            {
                var exp = libraryRepository.GetRegularExpressionById(id.ToString());
                this.RegularExpression = exp;
                this.IsAddingNewItem = false;
            }
            else
            {
                var regexPattern = navigationContext.Parameters["Add"];
                if (regexPattern != null)
                {
                    var exp = new RegularExpression
                    {
                        Expression = regexPattern.ToString()
                    };
                    this.RegularExpression = exp;
                    this.IsAddingNewItem = true;
                }
            }
            this.navigationJournal = navigationContext.NavigationService.Journal;
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }
    }
}
