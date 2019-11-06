using RodskaNote.Models;
using RodskaNote.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Providers
{
    public class RodskaLoader
    {
        [ImportMany(typeof(WorldDocument))]
        

        public List<WorldDocument> WorldDocuments { get; set; }
        [ImportMany(typeof(TreeEntry))]
        public List<TreeEntry> TreeEntries { get; set; }
        public List<Lazy<IRodskaNotePlugin, IRodskaNotePluginInfo>> RodskaNotePlugins { get; set; }
        private DirectoryCatalog pluginCatalog;
        private CompositionContainer pluginContainer;
        private AggregateCatalog mainCatalog;
        public RodskaLoader()
        {
            WorldDocuments = new List<WorldDocument>();
            TreeEntries = new List<TreeEntry>();
            pluginCatalog = new DirectoryCatalog(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Plugins"),"*.dll");
            mainCatalog = new AggregateCatalog();
            mainCatalog.Catalogs.Add(pluginCatalog);
            mainCatalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"*.exe"));
            mainCatalog.Catalogs.Add(new DirectoryCatalog(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "RodskaNote.SDK.dll"));

        }


        public void Load()
        {
            Console.WriteLine("Loading Plugins...");
            pluginContainer = new CompositionContainer(mainCatalog);
            pluginContainer.ComposeParts(this);
        }

        public void Unload()
        {
            pluginCatalog.Dispose();
            pluginCatalog = null;
            WorldDocuments.Clear();
            TreeEntries.Clear();
            WorldDocuments = null;
        }
    }
}
