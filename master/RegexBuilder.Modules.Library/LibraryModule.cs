using Microsoft.Practices.Prism.MefExtensions.Modularity;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using RegexBuilder.Modules.Library.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexBuilder.Modules.Library
{
    [ModuleExport("LibraryModule", typeof(LibraryModule))]
    //[ModuleExport("LibraryModule", typeof(LibraryModule), InitializationMode=InitializationMode.OnDemand)]
    public class LibraryModule : IModule
    {
        [Import]
        public IRegionManager regionManager;

        public void Initialize()
        {
            //this.regionManager.RegisterViewWithRegion("MainRegion", typeof(LibraryView));
            //this.regionManager.RequestNavigate("MainRegion", new Uri("LibraryView", UriKind.Relative));
            //this.regionManager.RequestNavigate("MainRegion", new Uri("ManipulationView?Edit=E-Mail", UriKind.Relative));
        }
    }
}
