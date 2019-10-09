using Catel;
using Catel.Data;
using Catel.MVVM;
using RodskaNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.ViewModels
{
    public class InteractionViewModel: ViewModelBase
    {
        [Model]
        public InteractionLiteral Interaction
        {
            get { return GetValue<InteractionLiteral>(InteractionProperty);  }
            set { SetValue(InteractionProperty, value);  }
        }

        public static readonly PropertyData InteractionProperty = RegisterProperty("Interaction", typeof(InteractionLiteral), null);

        public InteractionViewModel(InteractionLiteral interaction)
        {
            Argument.IsNotNull(() => interaction);
            Interaction = interaction;
        }

        [ViewModelToModel("Interaction")]
        public new string Title
        {
            get { return GetValue<string>(InteractionTitleProperty); }
            set { SetValue(InteractionTitleProperty, value); }

        }

        public static readonly PropertyData InteractionTitleProperty = RegisterProperty("Title", typeof(string), null);


        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }

    }
}
