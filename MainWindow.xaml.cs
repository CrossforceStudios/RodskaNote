using AdaptiveCards;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Runtime.Serialization;
using Catel.Runtime.Serialization.Xml;
using Catel.Windows;
using FontAwesome5;
using Microsoft.Win32;
using NodeNetwork.Toolkit.NodeList;
using Orc.Memento;
using ReactiveUI;
using RodskaNote.App;
using RodskaNote.Attributes;
using RodskaNote.EventSystem;
using RodskaNote.Models;
using RodskaNote.Providers;
using RodskaNote.ViewModels;
using Syncfusion.Windows.Tools;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using System.Xml;

namespace RodskaNote.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RodskaNote.App.Controls.RodskaWindow, IViewFor<MainEditorViewModel>
    {
#pragma warning disable IDE0052 // Remove unread private members
        private MasterViewModel vm;
#pragma warning restore IDE0052 // Remove unread private members
        private string documentTitle;

        public event EventHandler DocumentChanged;
        public EFontAwesomeIcon PasteIcon { get; set; } = EFontAwesomeIcon.Solid_Paste;

        public EFontAwesomeIcon CutIcon { get; set; } = EFontAwesomeIcon.Solid_Cut;

        public EFontAwesomeIcon CopyIcon { get; set; } = EFontAwesomeIcon.Solid_Copy;

        public EFontAwesomeIcon SelectAllIcon { get; set; } = EFontAwesomeIcon.Solid_ClipboardCheck;

        public EFontAwesomeIcon SaveIcon { get; set; } = EFontAwesomeIcon.Solid_Save;

        public EFontAwesomeIcon OpenIcon { get; set; } = EFontAwesomeIcon.Solid_FolderOpen;
        public RoutedUICommand SelectAllCommand { get; set; } = ApplicationCommands.SelectAll;
        public RoutedUICommand PasteCommand { get; set; } = ApplicationCommands.Paste;
        public RoutedUICommand CutCommand { get; set; } = ApplicationCommands.Cut;

        public EFontAwesomeIcon PluginIcon { get; set; } = EFontAwesomeIcon.Solid_Plug;
        public EFontAwesomeIcon DocumentIcon { get; } = EFontAwesomeIcon.Regular_File;

        public EFontAwesomeIcon CompileIcon { get; } = EFontAwesomeIcon.Solid_Code;
        public RoutedUICommand CopyCommand { get; set; } = ApplicationCommands.Copy;

        public RoutedUICommand FloatCommand { get; set; } = new RoutedUICommand("Float", "FloatTab", typeof(MainWindow));
        public EFontAwesomeIcon UndoIcon { get; set; } = EFontAwesomeIcon.Solid_Undo;

        public EFontAwesomeIcon RedoIcon { get; set; } = EFontAwesomeIcon.Solid_Redo;

        public RoutedUICommand CompileCurrentDocument { get; set; } = new RoutedUICommand("Compile Document","CompileDocument",typeof(MainWindow));

        private Dictionary<Type, EFontAwesomeIcon> documentIcons;
        public string DisplayType { get; set; }
        public string Version
        {
            get {
                RodskaApp app = (RodskaApp)RodskaApp.Current;
                return app.Version;
           }
        }
        public MainWindow()
        {
            vm = (MasterViewModel)new MasterViewModel(Dispatcher.CurrentDispatcher);
            documentTitle =  "RodskaNote";
            InitializeComponent();
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            this.ViewModel = new MainEditorViewModel();
            documentIcons = new Dictionary<Type, EFontAwesomeIcon>();
            foreach (Type t in app.GetLoadedDocumentTypes())
            {
                MethodInfo method = t.GetMethod("PopulateEditor", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (method != null) {
                    method.Invoke(null, new object[] { new Dictionary<string,Dictionary<string,object>>(){
                    {"Invoker", new Dictionary<string, object>()
                    {
                        {"Nodes",ViewModel.ListViewModel},
                        {"NodeModel",editorNodes }

                    } } } });
                    
                }
            }
            foreach(CreativeDocumentRepresentation cdm in app.InteractionModels)
            {
                documentIcons.Add(cdm.ObjectType, cdm.Icon);
                CreateDocButton(cdm);
            }
            foreach (CreativeDocumentRepresentation cdm in app.UtilityModels)
            {
                documentIcons.Add(cdm.ObjectType, cdm.Icon);
                CreateDocButton(cdm);
            }
            foreach (CreativeDocumentRepresentation cdm in app.ProgressionModels)
            {
                documentIcons.Add(cdm.ObjectType, cdm.Icon);
                CreateDocButton(cdm);
            }
            CreateInteractions.ApplyTemplate();
            CreateUtility.ApplyTemplate();
            this.WhenActivated(d =>
            {
               this.OneWayBind(ViewModel, vm => vm.ListViewModel, v => v.editorNodes.ViewModel).DisposeWith(d);

            });
            app.DocumentCreated += SetDocument;
        }

        private void SetDocument(object sender, WorldDocumentEventArgs e)
        {
            CurrentDocument = e.NewDocument;
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
            typeof(MainEditorViewModel), typeof(MainWindow), new PropertyMetadata(null));
        public new MainEditorViewModel ViewModel
        {
            get => (MainEditorViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public Visibility IsGraphCTVisible => CurrentDocument != null ? Visibility.Visible : Visibility.Hidden;

     
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (MainEditorViewModel)value;
        }
        public WorldDocument CurrentDocument
        {
            get { return vm.CurrentDocument; }
            set {
                vm.CurrentDocument = value;
                CurrentDocumentTitle = vm.CurrentDocument.Title;
                Type t = vm.CurrentDocument.GetType();
                List<string> newProps = EditProvider.GetUnavailableProperties(t);
                DocumentGrid.HidePropertiesCollection = new ObservableCollection<string>(newProps);
                DocumentGrid.SelectedObject = vm.CurrentDocument;
                 t.GetMethod("CreateEditor", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null,new object[] { currentWorldDocument});
                if (documentIcons.ContainsKey(t))
                {
                    // currentWorldDocument.IconSource = ImageAwesome.CreateImageSource(documentIcons[t], Brushes.Black, emSize: 16);
                }

            }

        }

        
        public void CreateDocButton(CreativeDocumentRepresentation cdr)
        {
            RibbonButton cdrButton = new RibbonButton();
            cdrButton.Label = cdr.Title;
            cdrButton.Command = cdr.CreateDocument;
            cdrButton.SizeForm = cdr.SizeForm;
            cdrButton.SmallIcon = ImageAwesome.CreateImageSource(cdr.Icon, Brushes.Black);
            cdrButton.LargeIcon = ImageAwesome.CreateImageSource(cdr.Icon, Brushes.Black);
            ScreenTip cdrTip = new ScreenTip();
            cdrTip.Description = cdr.Title;
            cdrTip.Content = cdr.Description;
            cdrButton.ToolTip = cdrTip;
            switch (cdr.Usage)
            {
                case DocumentUsage.Interaction:
                    CreateInteractions.Items.Add(cdrButton);
                    break;
                case DocumentUsage.Utility:
                    CreateUtility.Items.Add(cdrButton);
                    break;
                case DocumentUsage.Progression:
                    CreateProgression.Items.Add(cdrButton);
                    break;
                default:
                    Console.WriteLine(cdr.Usage + " is not valid.");
                    break;
            }
            
        }
        public string CurrentDocumentTitle
        {
            get { return documentTitle; } set { 
                documentTitle = value;
            }
        } 

        public ObservableCollection<WorldDocument> OpenDocuments
        {
            get { return vm.OpenDocuments; }
            private set { vm.OpenDocuments = value; }
        }

        public void SignalDocumentChanged(WorldDocument document)
        {
            DocumentChanged?.Invoke(document, new EventArgs());
        } 

       
        private void CanSaveCurrentDocument(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (CurrentDocument != null);
        }

        private void SaveCurrentDocument(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentDocument.SaveDocumentAs(vm.GetTypeFactory());
        }

        private void CanOpenNewDocument(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CompileCurrentDoc(object sender, EventArgs e)
        {
            CurrentDocument.Compile();
        }




        private void OpenNewDocument(object sender, ExecutedRoutedEventArgs e)
        {
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            List<Type> t = app.GetLoadedDocumentTypes();
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Rodska Note Documents (.rndml)|*.rndml",
                DefaultExt = ".rndml",
                Title = "Open RodskaNote Document..."
            };
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                XmlSerializer seri = (XmlSerializer)SerializationFactory.GetXmlSerializer();
                
                FileStream stream = File.Open(dlg.FileName, FileMode.Open);
                stream.Position = 0;
                StreamReader read = new StreamReader(stream);
                _ = read.ReadToEnd();
                stream.Position = 0;
                dynamic doc;
                XmlDocument docX = new XmlDocument();
                docX.Load(stream);
                var node = docX.DocumentElement.SelectSingleNode("Type");
                string dType = node.InnerText;
                if (app.TypeRef.ContainsKey(dType))
                {
                    Console.WriteLine("Using Doc Type.");
                    MethodInfo method = app.TypeRef[dType].GetMethod("LoadFromFile", BindingFlags.Public | BindingFlags.Static);
                    // stream.Position = 0;
                    doc = method.Invoke(null, new object[] { seri, stream });
                    Console.WriteLine("Ready!");
                    CurrentDocument = doc;
                }
                stream.Dispose();
                read.Dispose();
            }
        }

        private void OpenNewDocumentCard(object sender, ExecutedRoutedEventArgs e)
        {
            XmlSerializer seri = (XmlSerializer)SerializationFactory.GetXmlSerializer();
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            AdaptiveCard card = (AdaptiveCard)sender;
            FileStream stream = File.Open((string)card.AdditionalProperties["FileName"], FileMode.Open);
            stream.Position = 0;
            StreamReader read = new StreamReader(stream);
            _ = read.ReadToEnd();
            stream.Position = 0;
            dynamic doc;
            XmlDocument docX = new XmlDocument();
            docX.Load(stream);
            var node = docX.DocumentElement.SelectSingleNode("Type");
            string dType = node.InnerText;
            if (app.TypeRef.ContainsKey(dType))
            {
                Console.WriteLine("Using Doc Type.");
                MethodInfo method = app.TypeRef[dType].GetMethod("LoadFromFile", BindingFlags.Public | BindingFlags.Static);
                // stream.Position = 0;
                doc = method.Invoke(null, new object[] { seri, stream });
                Console.WriteLine("Ready!");
                CurrentDocument = doc;
            }
            stream.Dispose();
            read.Dispose();
        }

        private void OpenNewDocumentCmd(object sender, ExecutedRoutedEventArgs e)
        {
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            XmlSerializer seri = (XmlSerializer)SerializationFactory.GetXmlSerializer();
            string filename = (string)sender;
            FileStream stream = File.Open(filename, FileMode.Open);
            stream.Position = 0;
            StreamReader read = new StreamReader(stream);
            _ = read.ReadToEnd();
            stream.Position = 0;
            dynamic doc;
            XmlDocument docX = new XmlDocument();
            docX.Load(stream);
            var node = docX.DocumentElement.SelectSingleNode("Type");
            string dType = node.InnerText;
            if (app.TypeRef.ContainsKey(dType))
            {
                Console.WriteLine("Using Doc Type.");
                MethodInfo method = app.TypeRef[dType].GetMethod("LoadFromFile", BindingFlags.Public | BindingFlags.Static);
                // stream.Position = 0;
                doc = method.Invoke(null, new object[] { seri, stream });
                Console.WriteLine("Ready!");
                CurrentDocument = doc;
            }
            stream.Dispose();
            read.Dispose();
        }
        private void CompileNewDoc(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentDocument.Compile();
        }

        private void MainLayout_IsActiveWindowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WhiteboardTabs.IsGroupVisible = DockingManager.GetIsActiveWindow(Whiteboard);
        }

        private void PerformUndo(object sender, ExecutedRoutedEventArgs e)
        {
            var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
            mementoService.Undo();
        }

        private void CanPerformUndo(object sender, CanExecuteRoutedEventArgs e)
        {
            var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();

            e.CanExecute = mementoService.CanUndo;
        }

        private void PerformRedo(object sender, ExecutedRoutedEventArgs e)
        {
            var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();
            mementoService.Redo();
        }

        private void CanPerformRedo(object sender, CanExecuteRoutedEventArgs e)
        {
            var mementoService = ServiceLocator.Default.ResolveType<IMementoService>();

            e.CanExecute = mementoService.CanRedo;
        }
    }
}
