using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Attributes
{
    /// <summary>
    /// Used for actual population of the creative controls.
    /// </summary>
    public class CreativeDocumentRepresentation : ValidatableModelBase
    {
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof(string), null);

        public string Title
        {
            get { return GetValue<string>(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly PropertyData TitleProperty = RegisterProperty("Title", typeof(string), null);

        public string Description
        {
            get { return GetValue<string>(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly PropertyData DescriptionProperty = RegisterProperty("Description", typeof(string), null);

        public DocumentUsage Usage
        {
            get { return GetValue<DocumentUsage>(UsageProperty); }
            set { SetValue(UsageProperty, value); }
        }

        public static readonly PropertyData UsageProperty = RegisterProperty("Usage", typeof(DocumentUsage), () => DocumentUsage.Interaction);

        public Func<IUIVisualizerService, MasterViewModel, Task> CreationCommand
        {
            get { return GetValue<Func<IUIVisualizerService, MasterViewModel, Task>>(CreationCommandProperty); }
            set { SetValue(CreationCommandProperty, value); }
        }

        public Type ObjectType
        {
            get { return GetValue<Type>(ObjectTypeProperty); }
            set { SetValue(ObjectTypeProperty, value); }
        }

        public static readonly PropertyData ObjectTypeProperty = RegisterProperty("ObjectType", typeof(Type), null);

        public static readonly PropertyData CreationCommandProperty = RegisterProperty("CreationCommand", typeof(Func<IUIVisualizerService, MasterViewModel, Task>), null);

        public CreativeDocumentRepresentation()
        {
            CreateDocument = new TaskCommand(CreateDocumentAsync);
        }
        public TaskCommand CreateDocument { get; private set; }
        private async Task CreateDocumentAsync()
        {
            App app = (App)App.Current;
            await CreationCommand.Invoke(app.uiVisualizerService, app.currentMainVM);
        }
    }
}
