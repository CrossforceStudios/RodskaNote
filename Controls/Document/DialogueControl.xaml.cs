using Catel.MVVM;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using NodeNetwork.Toolkit.Layout.ForceDirected;
using NodeNetwork.Views;
using RodskaNote.Models;
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

namespace RodskaNote.Controls.Document
{
    /// <summary>
    /// Interaction logic for DialogueControl.xaml
    /// </summary>
    public partial class DialogueControl : UserControl
    {
       public IHighlightingDefinition XSHD { get; set; }

        public DialogueControl(IViewModel viewModel): base()
        {
            InitializeComponent();
            Uri uri = new Uri("Syntaxes/Lua.xshd", UriKind.Relative);
            StreamResourceInfo info = Application.GetResourceStream(uri);
            using (XmlTextReader reader = new XmlTextReader(info.Stream))
            {
                XSHD = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                HighlightingManager.Instance.RegisterHighlighting("Lua", new string[] { ".lua" }, XSHD);
                compilationLua.SyntaxHighlighting = HighlightingManager.Instance.GetDefinitionByExtension(".lua");

            }
            
        }

        public NetworkView ConversationWorkspace
        {
            get { return conversationWorkspace; }
        }

        private WorldDocument worldDocument;

        public WorldDocument CurrentDocument
        {
            get { return worldDocument;  }
            set {
                worldDocument = value;          
            }
        }

        private void Compiler_Click(object sender, RoutedEventArgs e)
        {
            App app = (App)App.Current;
            MainWindow window = (MainWindow)app.MainWindow;
            Dialogue doc = window.CurrentDocument as Dialogue;
            doc.Compile();
            compilationLua.Text = doc.CompilationResult;
        }

        private void LayoutO_Click(object sender, RoutedEventArgs e)
        {
            ForceDirectedLayouter layouter = new ForceDirectedLayouter();
            var config = new Configuration
            {
                Network = ConversationWorkspace.ViewModel,
            };
            layouter.Layout(config, 20000);
        }
    }
}
