namespace RodskaNote.ViewModels
{
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;
    using System.Threading.Tasks;
    using Catel.IoC;

    public class DialogueViewModel : ViewModelBase
    {
        [Model]
        public Dialogue Dialogue
        {
            get { return GetValue<Dialogue>(DialogueProperty);  }
            set { SetValue(DialogueProperty, value);  }
        }

        public static readonly PropertyData DialogueProperty = RegisterProperty("Dialogue", typeof(Dialogue), null);
        public DialogueViewModel(Dialogue dialogue)
        {
            Argument.IsNotNull(() => dialogue);
            Dialogue = dialogue;
        }



        [ViewModelToModel("Dialogue")]
        public new string Title
        {
            get { return GetValue<string>(DialogueTitleProperty); }
            set { SetValue(DialogueTitleProperty, value);  }

        }

        public static readonly PropertyData DialogueTitleProperty = RegisterProperty("Title", typeof(string), null);

        [ViewModelToModel("Dialogue")]
        public float ConversationDistance
        {
            get { return GetValue<float>(ConversationDistanceProperty); }
            set { SetValue(ConversationDistanceProperty, value); }

        }

        public static readonly PropertyData ConversationDistanceProperty = RegisterProperty("ConversationDistance", typeof(float), () => 25.0f);

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
