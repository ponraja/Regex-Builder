using Microsoft.Practices.Prism.MefExtensions;
using Microsoft.Practices.Prism.Modularity;
using RegexBuilder.Infrastructure.Utility;
using RegexBuilder.Modules.Analyzer;
using RegexBuilder.Modules.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.PubSubEvents;

namespace RegexBuilder
{
    public class RegexBuilderBootstrapper : MefBootstrapper
    {
        LibraryRepository libraryRepository;

        protected override DependencyObject CreateShell()
        {
            return this.Container.GetExportedValue<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            var eventAggregator = Container.GetExportedValue<IEventAggregator>();
            libraryRepository = new LibraryRepository(eventAggregator);
            libraryRepository.InitializeRepository();
            Container.ComposeExportedValue(libraryRepository);
            Application.Current.MainWindow = (Window)this.Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureAggregateCatalog()
        {
            base.ConfigureAggregateCatalog();

            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(RegexBuilderBootstrapper).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(AnalyzerModule).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(LibraryModule).Assembly));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            Container.ComposeExportedValue(Container);
        }
    }
}
