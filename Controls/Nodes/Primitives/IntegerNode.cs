using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using NodeNetwork.Views;
using ReactiveUI;
using RodskaNote.ViewModels.EditorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Controls.Nodes.Primitives
{
   public class IntegerNode: PrimitiveNode
    {
        static IntegerNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new PrimitiveNodeView(), typeof(IViewFor<IntegerNode>));
        }

        public IntegerValueEditorViewModel ValueEditor { get; } = new IntegerValueEditorViewModel();

        public ValueNodeOutputViewModel<int> Output { get; }

        public IntegerNode() 
        {
            Name = "Integer";

            Output = new ValueNodeOutputViewModel<int>()
            {
                Name = "Value",
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged.Select(v => v ?? 0)
            };
            this.Outputs.Add(Output);
        }
    }
}
