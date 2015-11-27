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
    /// Interaction logic for LibraryView.xaml
    /// </summary>
    [Export("LibraryView")]
    public partial class LibraryView : UserControl
    {
        [Import]
        public LibraryViewModel ViewModel
        {
            set { this.DataContext = value; }
        }

        public LibraryView()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            row.Focusable = true;
            bool focused = row.Focus();
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
            if (elementWithFocus != null)
                elementWithFocus.MoveFocus(request);
        }
    }
}
