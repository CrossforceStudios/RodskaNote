using NodeNetwork.Toolkit.NodeList;
using RodskaNote.Controls.Nodes.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.ViewModels
{
    public class MainEditorViewModel: ReactiveUI.ReactiveObject
    {
        public NodeListViewModel ListViewModel { get; } = new NodeListViewModel();

       public MainEditorViewModel()
        {
            ListViewModel.AddNodeType(() => new StringNode());
            ListViewModel.AddNodeType(() => new IntegerNode());

        }
    }
}
