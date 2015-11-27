using RegexBuilder.Modules.Library.ViewModels;
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

namespace RegexBuilder.Modules.Library.Views
{
    /// <summary>
    /// Interaction logic for ManipulationView.xaml
    /// </summary>
    [Export("ManipulationView")]
    public partial class ManipulationView : UserControl
    {
        [Import]
        public ManipulationViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        public ManipulationView()
        {
            InitializeComponent();
        }
    }
}
