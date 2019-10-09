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
   public class FloatNode: PrimitiveNode
    {
        static FloatNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new PrimitiveNodeView(), typeof(IViewFor<FloatNode>));
        }

        public FloatValueEditorViewModel ValueEditor { get; } = new FloatValueEditorViewModel();

        public ValueNodeOutputViewModel<float> Output { get; }

        public FloatNode() 
        {
            Name = "Float";

            Output = new ValueNodeOutputViewModel<float>()
            {
                Name = "Value",
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged.Select(v => v ?? 10.0f)
            };
            this.Outputs.Add(Output);
        }
    }
}
