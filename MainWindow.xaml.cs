using Catel.MVVM;
using Catel.Windows;
using Fluent;
using RodskaNote.Attributes;
using RodskaNote.Models;
using RodskaNote.Providers;
using RodskaNote.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace RodskaNote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RodskaNote.Controls.RodskaWindow
    {
#pragma warning disable IDE0052 // Remove unread private members
        private MasterViewModel vm;
#pragma warning restore IDE0052 // Remove unread private members
        private string documentTitle;
        public MainWindow()
        {
            vm = (MasterViewModel)new MasterViewModel(Dispatcher.CurrentDispatcher);
            InitializeComponent();
            CurrentDocumentTitle = "RodskaNote";

        }

        public WorldDocument CurrentDocument
        {
            get { return vm.CurrentDocument; }
            set { 
                vm.CurrentDocument = value;
                DocumentGrid.SelectedObject = vm.CurrentDocument;
                CurrentDocumentTitle = vm.CurrentDocument.Title;
                DocumentGrid.PropertyDefinitions = new PropertyDefinitionCollection();
                DocumentGrid.PropertyDefinitions.Add(new PropertyDefinition()
                {
                    TargetProperties = new List<string>()
                        {
                            "Title"
                        },
                    Category = "General",
                    Description = "The name of the document.",
                    DisplayName = "Document Title",
                    DisplayOrder = 0,
                });
                Type t = vm.CurrentDocument.GetType();
                PropertyDefinitionCollection newProps = EditProvider.GetAvailableProperties(t);
                foreach(PropertyDefinition prop in newProps)
                {
                    DocumentGrid.PropertyDefinitions.Add(prop);
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

        private void DocumentGrid_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            CurrentDocumentTitle = CurrentDocument.Title;
        }
    }
}
