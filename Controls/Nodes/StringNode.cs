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
    class StringNode: NodeViewModel
    {
        public StringValueEditorViewModel ValueEditor { get; } = new StringValueEditorViewModel();


        public ValueNodeOutputViewModel<string> StringOutput { get; set; } = new ValueNodeOutputViewModel<string>()
        {
            Name = "String Result"
        };

        static StringNode()
        {
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<StringNode>));
        }

        public StringNode()
        {
            Name = "String (Text)";

            StringOutput = new ValueNodeOutputViewModel<string>()
            {
                Name = "Value",
                Editor = ValueEditor,
                Value = ValueEditor.ValueChanged.Select(v => v)
            };
            this.Outputs.Add(StringOutput);
        }
    }
}
