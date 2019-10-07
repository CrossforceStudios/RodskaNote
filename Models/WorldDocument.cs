using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.AvalonDock.Layout;

namespace RodskaNote.Models
{
    /// <summary>
    /// An abstract class used for storing and implementing world design element types in RodskaNote.
    /// </summary>
    public abstract class WorldDocument : SavableModelBase<WorldDocument>
    {
        /// <summary>
        /// The heading of the document as displayed in the UI.
        /// </summary>
        public string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string), () => "Untitled");


#pragma warning disable IDE0060 // Remove unused parameter
        public static void InitializeDocumentType(IUIVisualizerService uiVisualizerService, IViewModelLocator viewModelLocator)
        {
#pragma warning restore IDE0060 // Remove unused parameter
        
        }

        public abstract void Compile();

        public string CompilationResult { get; set; }
        public static void CreateEditor(LayoutDocument document)
        {
            
        }

        public static void PopulateEditor(Dictionary<string, Dictionary<string, object>> details) { }
    }
}
