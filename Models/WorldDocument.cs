using Catel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Models
{
    /// <summary>
    /// An abstract class used for storing and implementing world design element types in RodskaNote.
    /// </summary>
    public abstract class WorldDocument : SavableModelBase<WorldDocument>
    {
        /// <summary>
        /// The heading of the document as displayed in the UI.
        /// </summary>
        public string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string), () => "Untitled");

    }
}
