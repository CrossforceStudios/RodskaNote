using Catel.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Windows.Controls;
using Catel.MVVM;
using System.ComponentModel;

namespace RodskaNote.Controls
{
    /// <summary>
    /// Control used for editing WorldDocuments
    /// </summary>
    [ToolboxItem(true)]
    public class WorldDocumentEditingControl: UserControl
    {
        public WorldDocumentEditingControl(IViewModel viewModel) {

        }

        /// <summary>
        /// Used for keeping track of the world document being used for editing.
        /// </summary>
        public WorldDocumentEditingControl CurrentDocument
        {
            get;
            private set;
        }

        
    }
}
