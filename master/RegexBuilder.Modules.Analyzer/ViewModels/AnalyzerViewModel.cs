using Microsoft.Practices.Prism.Commands;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace RegexBuilder.Modules.Analyzer.ViewModels
{
    [Export]
    public class AnalyzerViewModel : BindableBase
    {
        #region Variable

        FlowDocument _inputFlowDocument;
        string _inputText;
        string _regexPattern;
        string _status;
        bool isInProgress;
        private CompositionContainer Container;
        private IEventAggregator eventAggregator;
        private LibraryRepository libraryRepository;
        private ObservableCollection<RegularExpression> _regularExpression;
        
        #endregion

        #region Property

        [Import]
        public IRegionManager RegionManager { get; set; }

        public FlowDocument InputFlowDocument
        {
            get { return _inputFlowDocument; }
            set { SetProperty(ref _inputFlowDocument, value); }
        }

        public string InputText 
        {
            get { return _inputText; }
            set { SetProperty(ref _inputText, value); }
        }

        public string RegexPattern
        {
            get { return _regexPattern; }
            set { SetProperty(ref _regexPattern, value); }
        }

        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }

        public ObservableCollection<RegularExpression> RegularExpressions
        {
            get { return _regularExpression; }
            set { SetProperty(ref _regularExpression, value); }
        }

        #endregion

        #region Commands

        public ICommand ValidateRegex { get; private set; }
        public ICommand OpenLibrary { get; private set; }
        public ICommand AddRegularExpressionCommand { get; private set; }

        #endregion

        #region Ctor

         [ImportingConstructor]
        public AnalyzerViewModel(CompositionContainer container, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.Container = container;
            InputFlowDocument = new FlowDocument();
            ValidateRegex = new DelegateCommand(OnValidateRegex, CanExecuteValidateRegex);
            OpenLibrary = new DelegateCommand(OnOpenLibrary);
            AddRegularExpressionCommand = new DelegateCommand(AddRegularExpressionCommandHandler);

            libraryRepository = container.GetExportedValue<LibraryRepository>();
            RegularExpressions = libraryRepository.RegularExpressions;
            this.eventAggregator.GetEvent<UpdateRegularExpressionCollectionEvent>().Subscribe(UpdateRegularExpressionCollectionEventHandler);
            eventAggregator.GetEvent<SelectedRegularExpressionEvent>().Subscribe(SelectedRegularExpressionEventHandler);
        }

         private void UpdateRegularExpressionCollectionEventHandler(NotifyCollectionChangedAction obj)
         {
             RegularExpressions = libraryRepository.RegularExpressions;
         }

        private void SelectedRegularExpressionEventHandler(RegularExpression exp)
        {
            if (exp != null)
            {
                RegexPattern = exp.Expression;
            }
        }

        private void OnOpenLibrary()
        {
            this.RegionManager.RequestNavigate("MainRegion", new Uri("LibraryView", UriKind.Relative));
        }

        private void AddRegularExpressionCommandHandler()
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("Add", RegexPattern ?? string.Empty);
            this.RegionManager.RequestNavigate("MainRegion", new Uri("ManipulationView" + parameters, UriKind.Relative));
        }

        #endregion

        #region Command Handlers

        private bool CanExecuteValidateRegex()
        {
            return (!isInProgress && !string.IsNullOrEmpty(this.RegexPattern) && !string.IsNullOrWhiteSpace(this.RegexPattern)
                && !string.IsNullOrEmpty(this.InputText) && !string.IsNullOrWhiteSpace(this.InputText)
                && Helper.IsValidRegex(this.RegexPattern));
        }

        private void OnValidateRegex()
        {
            if(!string.IsNullOrEmpty(this.RegexPattern) && !string.IsNullOrWhiteSpace(this.RegexPattern)
                && !string.IsNullOrEmpty(this.InputText) && !string.IsNullOrWhiteSpace(this.InputText))
            {
                isInProgress = true;
                Regex reg = new Regex(this.RegexPattern);
                this.InputFlowDocument.Blocks.Clear();
                Paragraph myParagraph = new Paragraph();
                myParagraph.Inlines.Add(new Run(this.InputText));
                this.InputFlowDocument.Blocks.Add(myParagraph);

                var start = this.InputFlowDocument.ContentStart;
                while (start != null && start.CompareTo(this.InputFlowDocument.ContentEnd) < 0)
                {
                    if (start.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                    {
                        var match = reg.Match(start.GetTextInRun(LogicalDirection.Forward));
                        if (match.Success)
                        {
                            var textrange = new TextRange(start.GetPositionAtOffset(match.Index, LogicalDirection.Forward), start.GetPositionAtOffset(match.Index + match.Length, LogicalDirection.Backward));
                            textrange.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(Colors.Blue));
                            textrange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                            start = textrange.End;
                        }
                    }
                    start = start.GetNextContextPosition(LogicalDirection.Forward);
                }
                isInProgress = false;
            }
        }

        #endregion
    }
}
