using Catel.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Models
{
    /// <summary>
    /// Class used for storing all of a user's <see cref="Dialogue"/> documents.
    /// </summary>
    public class Dialogues : SavableModelBase<Dialogues>
    {
        /// <summary>
        /// Every <see cref="Dialogue"/> that a user has created and saved.
        /// </summary>
        public ObservableCollection<Dialogue> AllDialogues
        {
            get { return GetValue<ObservableCollection<Dialogue>>(DialoguesProperty); }
            set { SetValue(DialoguesProperty, value); }
        }

        public static readonly PropertyData DialoguesProperty = RegisterProperty("AllDialogues", typeof(ObservableCollection<Dialogue>),() => new ObservableCollection<Dialogue>());


    }
}
