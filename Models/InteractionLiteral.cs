using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using DynamicData;
using NodeNetwork.Views;
using ReactiveUI;
using RodskaNote.Attributes;
using RodskaNote.App.Controls.Document;
using RodskaNote.Controls.Nodes;
using RodskaNote.ViewModels;
using RodskaNote.Views;
using Xceed.Wpf.AvalonDock.Layout;
using RodskaNote.App;
using RodskaNote.Models;

namespace RodskaNote.App.Models
{
    public enum InteractionContext
    {
        Interaction,
        None
    }

    public enum ActionType
    {
        Press,
        Hold,
        None
    }
    [Export(typeof(WorldDocument))]
    [CreativeDocumentModel("InteractionLiteral","Interaction","Creates a keybind (gamepad and keyboard compatible) interaction that can be used to do tasks such as opening vehicle doors.",DocumentUsage.Interaction,Icon = FontAwesome.WPF.FontAwesomeIcon.Gamepad)]
   public class InteractionLiteral : WorldDocument
    {
        [PropertiesEnabled("Interaction Context", "The type of interaction.", "Contexts", 1)]
        public InteractionContext Context
        {
            get { return GetValue<InteractionContext>(ContextProperty); }
            set { SetValue(ContextProperty, value); }

        }

        public static readonly PropertyData ContextProperty = RegisterProperty("Context", typeof(InteractionContext), () => InteractionContext.None);
      
        [PropertiesEnabled("Action Type","Whether the interaction is activated via press or hold","Input",1)]
        public ActionType InputContext
        {
            get { return GetValue<ActionType>(ActionTypeProperty); }
            set { SetValue(ActionTypeProperty, value); }
        }

        public string ActionBuilderLua
        {
            get { return GetValue<string>(ActionBuilderLuaProperty); }
            set { SetValue(ActionBuilderLuaProperty, value);  }
        }

        public static readonly PropertyData ActionBuilderLuaProperty = RegisterProperty("ActionBuilderLua", typeof(string), null);

        public string CreationConditionLua
        {
            get { return GetValue<string>(CreationConditionLuaProperty); }
            set { SetValue(CreationConditionLuaProperty, value); }
        }

        public static readonly PropertyData CreationConditionLuaProperty = RegisterProperty("CreationConditionLua", typeof(string), null);

        public string ClientIntLua
        {
            get { return GetValue<string>(ClientIntLuaProperty); }
            set { SetValue(ClientIntLuaProperty, value); }
        }

        public static readonly PropertyData ClientIntLuaProperty = RegisterProperty("ClientIntLua", typeof(string), null);

        public string LocationLua
        {
            get { return GetValue<string>(LocationLuaProperty); }
            set { SetValue(LocationLuaProperty, value); }
        }

        public static readonly PropertyData LocationLuaProperty = RegisterProperty("LocationLua", typeof(string), null);

        public float ActivationDistance
        {
            get { return GetValue<float>(ActivationDistanceProperty); }
            set { SetValue(ActivationDistanceProperty, value); }
        }

        public static readonly PropertyData ActivationDistanceProperty = RegisterProperty("ActivationDistance", typeof(float), null);


        public static readonly PropertyData ActionTypeProperty = RegisterProperty("ActionTypeContext", typeof(ActionType), () => ActionType.None);

        public string Description
        {
            get { return GetValue<string>(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly PropertyData DescriptionProperty = RegisterProperty("Description", typeof(string), null);

        public override void Compile()
        {
            StringBuilder stringBuilder = new StringBuilder("return {\n");
            stringBuilder.Append($"\tName = \"{Title}\";\n");
            stringBuilder.Append($"\tContext = \"{Enum.GetName(typeof(InteractionContext),Context)}\";\n");
            stringBuilder.Append($"\tDescription = \"{Description}\";\n");
            stringBuilder.Append($"\tKeyContext = \"{"Regular"}\";\n");
            stringBuilder.Append($"\tActionType = \"{Enum.GetName(typeof(ActionType), InputContext)}\";\n");
            stringBuilder.Append($"\tActionBuilder = function(object)\n");
            stringBuilder.Append($"\t\t{ActionBuilderLua}\n");
            stringBuilder.Append($"\tend;\n");
            stringBuilder.Append($"\tCreationCondition = function(object)\n");
            stringBuilder.Append($"\t\t{CreationConditionLua}\n");
            stringBuilder.Append($"\tend;\n");
            stringBuilder.Append($"\tActivationDistance = {ActivationDistance};\n");
            stringBuilder.Append($"\tClientInt = function(object)\n");
            stringBuilder.Append($"\t\t{ClientIntLua}\n");
            stringBuilder.Append($"\tend;\n");
            stringBuilder.Append($"\tLocation = {LocationLua};\n");
            stringBuilder.Append($"}}");
            CompilationResult = stringBuilder.ToString();
        }

        public static ObservableCollection<InteractionLiteral> Interactions = new ObservableCollection<InteractionLiteral>();


        public override void SaveDocumentAs(ITypeFactory factory)
        {
            
        }

        public static new void InitializeDocumentType(IUIVisualizerService uiVisualizerService, IViewModelLocator viewModelLocator)
        {

            viewModelLocator.Register<InteractionView, InteractionViewModel>();
            viewModelLocator.Register<InteractionControl, InteractionViewModel>();

            uiVisualizerService.Register(typeof(InteractionViewModel), typeof(InteractionView), true);
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<InteractionNode>));

        }


        [STAThread]
        public static async Task CreateAsync(IUIVisualizerService _uiVisualizerService, MasterViewModel viewModel)
        {

            _ = viewModel.Dispatcher.Invoke(async () =>
            {
                InteractionLiteral interaction_new = new InteractionLiteral();
                ITypeFactory dialogueFactory = viewModel.GetTypeFactory();
                var interactionView = dialogueFactory.CreateInstanceWithParametersAndAutoCompletion<InteractionViewModel>(interaction_new);
                DocumentAdded += (object sender, UICompletedEventArgs e) =>
                {
                    if (!interaction_new.IsCanceled)
                    {
                        Console.WriteLine("[RodskaNote]: New Interaction - " + interaction_new.Title);
                        Interactions.Add(interaction_new);
                        RodskaApp app = (RodskaApp)RodskaApp.Current;
                        MainWindow window = (MainWindow)app.MainWindow;
                        window.CurrentDocument = interaction_new;
                    }
                };
                var result = await GetFromPrompt(_uiVisualizerService, interactionView);
                DocumentAdded -= (object sender, UICompletedEventArgs e) =>
                {
                    if (!interaction_new.IsCanceled)
                    {
                        Console.WriteLine("[RodskaNote]: New Interaction - " + interaction_new.Title);
                        Interactions.Add(interaction_new);
                        RodskaApplication app = (RodskaApplication)RodskaApplication.Current;
                        MainWindow window = (MainWindow)app.MainWindow;
                        window.CurrentDocument = interaction_new;
                    }
                };
            });


            await Task.CompletedTask;
        }

        public override string ToString()
        {
            return Title;
        }

        public InteractionLiteral()
        {
            Type = "InteractionLiteral";
            DisplayType  = "Keybind Interaction";
        }



        public static new string GetTypeString()
        {
            return "InteractionLiteral";
        }

        public static new void PopulateEditor(Dictionary<string, Dictionary<string, object>> details)
        {
           // NodeListViewModel listViewModel = details["Invoker"]["Nodes"] as NodeListViewModel;
        }
        public static void CreateEditor(LayoutDocument document)
        {
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            document.Content = null;
            ITypeFactory typeFactory = app.currentMainVM.GetTypeFactory();
            MainWindow mainWindow = (MainWindow)app.MainWindow;
            if (mainWindow.CurrentDocument == null)
            {
                return;
            }
            InteractionLiteral interaction = (InteractionLiteral)mainWindow.CurrentDocument;
            InteractionViewModel interactionView = typeFactory.CreateInstanceWithParametersAndAutoCompletion<InteractionViewModel>(interaction);
            InteractionControl control = new InteractionControl(interactionView);

            control.interactionWorkspace.ViewModel = new NodeNetwork.ViewModels.NetworkViewModel();
            InteractionNode node = new InteractionNode(interaction);

            control.interactionWorkspace.ViewModel.Nodes.Add(node);
            // node.Name = interaction.Title;
            // interactionView.conversationWorkspace.ViewModel.Nodes.Add(node);

            control.CurrentDocument = interaction;
            document.Content = control;



        }

    }
}
