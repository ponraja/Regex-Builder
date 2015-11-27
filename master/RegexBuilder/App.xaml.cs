using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.Globalization;
using FirstFloor.ModernUI.Presentation;
using System.Windows.Media;

namespace RegexBuilder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegexBuilderBootstrapper bootstrapper = new RegexBuilderBootstrapper();
            bootstrapper.Run();
            AppearanceManager.Current.AccentColor = Colors.Gray;
        }
    }
}
