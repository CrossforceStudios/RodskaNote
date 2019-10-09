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
    /// Interaction logic for DialogueResponseNodeView.xaml
    /// </summary>
    public partial class DialogueResponseNodeView : UserControl, IViewFor<DialogueResponseNode>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
          typeof(DialogueResponseNode), typeof(DialogueResponseNodeView), new PropertyMetadata(null));
        public DialogueResponseNode ViewModel
        {
            get => (DialogueResponseNode)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (DialogueResponseNode)value;
        }
        public DialogueResponseNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.NodeView.ViewModel).DisposeWith(d);
            });
        }
       
    }
}
