using Catel.IoC;
using Catel.MVVM;
using Catel.Runtime.Serialization;
using Catel.Runtime.Serialization.Xml;
using Catel.Windows;
using Fluent;
using FontAwesome.WPF;
using Microsoft.Win32;
using NodeNetwork.Toolkit.NodeList;
using ReactiveUI;
using RodskaNote.Attributes;
using RodskaNote.Models;
using RodskaNote.Providers;
using RodskaNote.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace RodskaNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RodskaNote.Controls.RodskaWindow, IViewFor<MainEditorViewModel>
    {
#pragma warning disable IDE0052 // Remove unread private members
        private MasterViewModel vm;
#pragma warning restore IDE0052 // Remove unread private members
        private string documentTitle;

        public event EventHandler DocumentChanged;
        public ImageSource PasteIcon { get; set; }

        public ImageSource CutIcon { get; set; }

        public ImageSource CopyIcon { get; set; }
        public RoutedUICommand PasteCommand { get; set; } = ApplicationCommands.Paste;
        public RoutedUICommand CutCommand { get; set; } = ApplicationCommands.Cut;

        public FontAwesomeIcon DocumentIcon { get; } = FontAwesomeIcon.File;

        public FontAwesomeIcon CompileIcon { get; } = FontAwesomeIcon.Code;
        public RoutedUICommand CopyCommand { get; set; } = ApplicationCommands.Copy;

        public RoutedUICommand FloatCommand { get; set; } = new RoutedUICommand("Float", "FloatTab", typeof(MainWindow));

        public RoutedUICommand CompileCurrentDocument { get; set; } = new RoutedUICommand("Compile Document","CompileDocument",typeof(MainWindow));

        private Dictionary<Type, FontAwesomeIcon> documentIcons;
        public string DisplayType { get; set; }
        public string Version
        {
            get {
                App app = (App)App.Current;
                return app.Version;
           }
        }
        public MainWindow()
        {
            vm = (MasterViewModel)new MasterViewModel(Dispatcher.CurrentDispatcher);
            InitializeComponent();
            CurrentDocumentTitle = "RodskaNote";
            App app = (App)App.Current;
            this.ViewModel = new MainEditorViewModel();
            documentIcons = new Dictionary<Type, FontAwesomeIcon>();
            PasteIcon = ImageAwesome.CreateImageSource(FontAwesomeIcon.Paste, Brushes.Black);
            CutIcon = ImageAwesome.CreateImageSource(FontAwesomeIcon.Cut, Brushes.Black);
            CopyIcon = ImageAwesome.CreateImageSource(FontAwesomeIcon.Copy, Brushes.Black);
            foreach (Type t in CreativeDocumentModel.GetDocumentTypes<WorldDocument>())
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
            }
            
            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel, vm => vm.ListViewModel, v => v.editorNodes.ViewModel).DisposeWith(d);

            });
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
                DocumentGrid.SelectedObject = vm.CurrentDocument;
                DocumentGrid.SelectedObjectTypeName = vm.CurrentDocument.DisplayType;
                DocumentGrid.SelectedObjectName = vm.CurrentDocument.ToString();
                CurrentDocumentTitle = vm.CurrentDocument.Title;
                Console.WriteLine(MainRibbon.ToolBarItems.Count);
                DocumentGrid.PropertyDefinitions = new PropertyDefinitionCollection
                {
                    new PropertyDefinition()
                    {
                        TargetProperties = new List<string>()
                        {
                            "Title"
                        },
                        Category = "General",
                        Description = "The name of the document.",
                        DisplayName = "Document Title",
                        DisplayOrder = 0,
                    }
                };
                Type t = vm.CurrentDocument.GetType();
                DisplayType = CurrentDocument.DisplayType;
                PropertyDefinitionCollection newProps = EditProvider.GetAvailableProperties(t);
                foreach(PropertyDefinition prop in newProps)
                {
                    DocumentGrid.PropertyDefinitions.Add(prop);
                }
                t.GetMethod("CreateEditor", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null,new object[] { currentWorldDocument});
                if (documentIcons.ContainsKey(t))
                {
                    currentWorldDocument.IconSource = ImageAwesome.CreateImageSource(documentIcons[t], Brushes.Black, emSize: 16);
                }

            }

        }


        public string CurrentDocumentTitle
        {
            get { return documentTitle; } set { 
                documentTitle = value;
                currentWorldDocument.Title = documentTitle;
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

        private void DocumentGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            CurrentDocumentTitle = CurrentDocument.Title;
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
            List<Type> t = CreativeDocumentModel.GetDocumentTypes<WorldDocument>();
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rodska Note Documents (.rndml)|*.rndml";
            dlg.DefaultExt = ".rndml";
            dlg.Title = "Open RodskaNote Document...";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                XmlSerializer seri = new XmlSerializer(new SerializationManager(),new DataContractSerializerFactory(), new XmlNamespaceManager(), vm.GetTypeFactory(), new ObjectAdapter());
                FileStream stream = new FileStream(dlg.FileName, FileMode.Open);
                WorldDocument doc = new Dialogue();
                foreach(Type type in t)
                {
                    MethodInfo method = type.GetMethod("GetTypeString", BindingFlags.Public | BindingFlags.Static);
                    if (method != null)
                    {
                        doc = (WorldDocument)seri.Deserialize(doc, stream);
                        if ((string)method.Invoke(null,null) == doc.Type)
                        {
                            CurrentDocument = doc;
                            stream.Close();
                            break;
                        }
                    }
                }   
            }
        }

        private void CompileNewDoc(object sender, ExecutedRoutedEventArgs e)
        {
            CurrentDocument.Compile();
        }

        private void Float_Click(object sender, RoutedEventArgs e)
        {
            LayoutContent item = RodskaRoot.ActiveContent as LayoutContent;
            item.Float();
        }

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            LayoutContent item = RodskaRoot.ActiveContent as LayoutContent;
            LayoutAnchorable anchorable = item as LayoutAnchorable;
            if (anchorable != null)
            {
                anchorable.Hide();
            }
            
        }
    }
}
