using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using NodeNetwork.Toolkit.Layout.ForceDirected;
using RodskaNote.App.Models;
using RodskaNote.Models;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Xml;

namespace RodskaNote.App.Controls.Document
{
    /// <summary>
    /// Interaction logic for InteractionControl.xaml
    /// </summary>
    public partial class InteractionControl : UserControl
    {
        public IHighlightingDefinition XSHD { get; set; }

        public InteractionControl(InteractionViewModel viewModel): base()
        {
            Uri uri = new Uri("Syntaxes/Lua.xshd", UriKind.Relative);
            InitializeComponent();
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using XmlTextReader reader = new XmlTextReader(info.Stream);
            XSHD = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            HighlightingManager.Instance.RegisterHighlighting("Lua", new string[] { ".lua" }, XSHD);
            serverEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".lua");
            compilationLua.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".lua");
        }

        private WorldDocument worldDocument;

        public WorldDocument CurrentDocument
        {
            get { return worldDocument; }
            set
            {
                worldDocument = value;
            }
        }

        private void Compiler_Click(object sender, RoutedEventArgs e)
        {
            RodskaApplication app = (RodskaApplication)RodskaApp.Current;
            MainWindow window = (MainWindow)app.MainWindow;
            InteractionLiteral doc = window.CurrentDocument as InteractionLiteral;
            doc.Compile();
            compilationLua.Text = doc.CompilationResult;
        }

        private void LayoutO_Click(object sender, RoutedEventArgs e)
        {
            ForceDirectedLayouter layouter = new ForceDirectedLayouter();
            var config = new Configuration
            {
                Network = interactionWorkspace.ViewModel,
            };
            layouter.Layout(config, 20000);
        }
    }
}
