using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Regions;
using RegexBuilder.Modules.Analyzer.Views;

namespace RegexBuilder.Modules.Analyzer
{
    [ModuleExport("AnalyzerModule", typeof(AnalyzerModule))]
    public class AnalyzerModule : IModule
    {
        [Import]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            this.RegionManager.RequestNavigate("MainRegion", new Uri("AnalyzerView", UriKind.Relative));
        }
    }
}
