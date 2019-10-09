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

namespace RodskaNote.Controls.Nodes.Primitives
{
    /// <summary>
    /// Interaction logic for PrimitiveNodeView.xaml
    /// </summary>
    public partial class PrimitiveNodeView : UserControl, IViewFor<PrimitiveNode>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
          typeof(PrimitiveNode), typeof(PrimitiveNodeView), new PropertyMetadata(null));
        public PrimitiveNode ViewModel
        {
            get => (PrimitiveNode)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PrimitiveNode)value;
        }
        public PrimitiveNodeView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.NodeView.ViewModel).DisposeWith(d);
            });
        }

    }
}
