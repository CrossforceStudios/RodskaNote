using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Runtime.Serialization;
using Catel.Runtime.Serialization.Xml;
using Catel.Services;
using Microsoft.Win32;
using RodskaNote.App.Controls.Document;
using RodskaNote.App.ViewModels;
using RodskaNote.Attributes;
using RodskaNote.Models;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shell;
using System.Xml;
using System.Xml.Serialization;

namespace RodskaNote.App.Models
{
    [Export(typeof(WorldDocument))]
    [CreativeDocumentModel("ProgressionTree", "Progression Tree", "Used for planning progression-related paths.", DocumentUsage.Progression, Icon = FontAwesome5.EFontAwesomeIcon.Solid_Award)]
    public class ProgressionTree : TreeDocumentNG
    {
        public override void Compile()
        {
            throw new NotImplementedException();
        }

        public override void SaveDocumentAs(ITypeFactory factory)
        {
            Catel.Runtime.Serialization.Xml.XmlSerializer seri = (Catel.Runtime.Serialization.Xml.XmlSerializer)SerializationFactory.GetXmlSerializer();
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Rodska Note Documents (.rndml)|*.rndml",
                FileName = "ProgressionTree",
                DefaultExt = ".rndml"
            };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                FileStream stream = new FileStream(dlg.FileName, FileMode.Create);
                this.Save(stream, seri);
                stream.Close();
                JumpList jl = JumpList.GetJumpList(RodskaApplication.Current);
                bool jpExisting = false;
                foreach (JumpItem item in jl.JumpItems)
                {
                    if (item is JumpPath)
                    {
                        JumpPath p = item as JumpPath;
                        if (p.Path == dlg.FileName)
                        {
                            jpExisting = true;
                            break;
                        }
                    }
                }
                if (!jpExisting)
                {
                    JumpPath jp = new JumpPath
                    {
                        Path = dlg.FileName,
                        CustomCategory = Type
                    };
                    jl.JumpItems.Add(jp);
                    JumpList.AddToRecentCategory(jp);
                    jl.Apply();
                }
            }
        }

        public ProgressionTree(): base()
        {
            this.RootNodeP = new ProgressionEntry();
            this.RootNodeP.Title = "Root";
            this.RootNodeP.Level = 0;
            this.RootNodeP.Children.Add(new ProgressionEntry()
            {
                Title = "Untitled Batch",
                Level = 1,
                ItemType = ProgressionTreeItemType.Root
            });
            this.Type = "Progression Tree";
        }

        public  ProgressionEntry RootNodeP
        {
            get { return GetValue<ProgressionEntry>(RootNodePProperty); }
            set
            {
                ProgressionEntry oldNode = GetValue<ProgressionEntry>(RootNodePProperty);
                ProgressionEntry newNode = value;
                TrackSet<ProgressionEntry>(RootNodePProperty, oldNode, newNode);
            }
        }

        public static readonly PropertyData RootNodePProperty = RegisterProperty("RootNodeP", typeof(ProgressionEntry), null);
 

        [STAThread]
        public static async Task CreateAsync(IUIVisualizerService _uiVisualizerService, MasterViewModel viewModel)
        {
            ProgressionTree progression = new ProgressionTree();
            ITypeFactory localizationFactory = viewModel.GetTypeFactory();
            RodskaApplication app = (RodskaApplication)RodskaApplication.Current;
            var localeVM = localizationFactory.CreateInstanceWithParametersAndAutoCompletion<ProgressTreeViewModel>(progression);
            viewModel.Dispatcher.Invoke(() =>
            {
                app.SetDocument(progression);
            });
            await Task.CompletedTask;
        }

        public static new void InitializeDocumentType(IUIVisualizerService uiVisualizerService, IViewModelLocator viewModelLocator)
        {
            viewModelLocator.Register<ProgressionTreeControl, ProgressTreeViewModel>();
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            app.AddType(GetTypeString(), typeof(ProgressionTree));
        }

        public static ProgressionTree LoadFromFile(Catel.Runtime.Serialization.Xml.XmlSerializer seri, FileStream stream)
        {
            ProgressionTree tree =  new ProgressionTree();
            try
            {
                stream.Position = 0;
                
                seri.Warmup(new List<Type>() { typeof(TreeEntry), typeof(ProgressionEntry),typeof(ProgressionTree)});
                tree = SavableModelBase<ProgressionTree>.Load(stream, seri);
            } catch(SerializationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            return tree;
           // return seri.Deserialize<ProgressionTree>(stream);
        }

        public static new void CreateEditor(ContentControl document)
        {
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            document.Content = null;
            ITypeFactory typeFactory = app.currentMainVM.GetTypeFactory();
            MainWindow mainWindow = (MainWindow)app.MainWindow;
            if (mainWindow.CurrentDocument == null)
            {
                return;
            }
            ProgressionTree tree = (ProgressionTree)mainWindow.CurrentDocument;
            ProgressTreeViewModel viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<ProgressTreeViewModel>(tree);
            ProgressionTreeControl control = new ProgressionTreeControl(viewModel);
            document.Content = control;
        }

        public static  new ProgressionTree Convert(object obj)
        {
            return (ProgressionTree)obj;
        }
        public static new string GetTypeString()
        {
            return "Progression Tree";
        }
    }
}
