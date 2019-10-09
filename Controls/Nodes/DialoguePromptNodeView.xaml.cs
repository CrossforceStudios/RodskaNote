using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
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
using System.Windows.Shapes;

namespace RodskaNote.Controls.Nodes
{
    /// <summary>
    /// Interaction logic for RodskaNodeView.xaml
    /// </summary>
    public partial class DialoguePromptNodeView : UserControl, IViewFor<DialoguePromptNode>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
           typeof(DialoguePromptNode), typeof(DialoguePromptNodeView), new PropertyMetadata(null));
        public DialoguePromptNode ViewModel
        {
            get => (DialoguePromptNode)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (DialoguePromptNode)value;
        }
        public DialoguePromptNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.NodeView.ViewModel).DisposeWith(d);
            });
        }
    }
}
