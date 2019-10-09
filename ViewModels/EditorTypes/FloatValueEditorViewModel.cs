using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using RodskaNote.Controls.Editor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.ViewModels.EditorTypes
{

    public class FloatValueEditorViewModel : ValueEditorViewModel<float?>
    {
        static FloatValueEditorViewModel()
        {
            Splat.Locator.CurrentMutable.Register(() => new FloatValueEditorView(), typeof(IViewFor<FloatValueEditorViewModel>));
        }

        public FloatValueEditorViewModel()
        {
            Value = 0.0f;
        }
    }
}
