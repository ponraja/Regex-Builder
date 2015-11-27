using RegexBuilder.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RegexBuilder.Infrastructure.Controls
{
    public class AutoCompleteTextBox : AutoCompleteBox
    {
        static AutoCompleteTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoCompleteTextBox), new FrameworkPropertyMetadata(typeof(AutoCompleteTextBox)));
        }

        public string WaterMarkText
        {
            get { return (string)GetValue(WaterMarkTextProperty); }
            set { SetValue(WaterMarkTextProperty, value); }
        }

        public static readonly DependencyProperty WaterMarkTextProperty =
            DependencyProperty.Register("WaterMarkText", typeof(string), typeof(AutoCompleteTextBox), new PropertyMetadata(null));

        public AutoCompleteTextBox()
        {
            this.ItemFilter = OnItemFilter;
        }

        private bool OnItemFilter(string search, object item)
        {
            var exp = item as RegularExpression;
            if (exp != null)
            {
                if (exp.Title.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (exp.CamelCaseTitle.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (exp.TitleWithoutSpecialChar.StartsWith(search, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
