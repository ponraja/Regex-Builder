using RegexBuilder.Modules.Analyzer.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegexBuilder.Modules.Analyzer.Views
{
    /// <summary>
    /// Interaction logic for AnalyzerView.xaml
    /// </summary>
    [Export("AnalyzerView")]
    public partial class AnalyzerView : UserControl
    {
        public AnalyzerView()
        {
            InitializeComponent();
        }

        [Import]
        public AnalyzerViewModel ViewModel
        {
            set { this.DataContext = value; }
        }
    }
}
