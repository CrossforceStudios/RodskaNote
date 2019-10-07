using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using DynamicData;
using NodeNetwork.Toolkit.NodeList;
using NodeNetwork.Views;
using ReactiveUI;
using RodskaNote.Attributes;
using RodskaNote.Controls.Document;
using RodskaNote.Controls.Nodes;
using RodskaNote.Models;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Layout;

namespace RodskaNote
{
    /// <summary>
    /// <see cref="WorldDocument"/> used for storing conversation interactions between a player and an NPC.
    /// </summary>
    [CreativeDocumentModel("Dialogue", "Conversation", "Used to make interactions between the player and an NPC.", DocumentUsage.Interaction)]
    public class Dialogue : WorldDocument
    {
        /// <summary>
        /// A float representing the distance at which the conversation can be held.
        /// </summary>
        [PropertiesEnabled("Conversation Distance", "The distance at which the dialogue can be held.", "Metrics", 1)]

        public float ConversationDistance
        {
            get => GetValue<float>(ConversationDistanceProperty);
            set => SetValue(ConversationDistanceProperty, value);
        }

        public static readonly PropertyData ConversationDistanceProperty = RegisterProperty("ConversationDistance", typeof(float), () => 25);


        public static ObservableCollection<Dialogue> Dialogues = new ObservableCollection<Dialogue>();
        /// <summary>
        /// A float representing the distance at which a conversation can be started automatically.
        /// </summary>
        [PropertiesEnabled("Trigger Distance", "The distance at which the dialogue can be started.", "Metrics", 2)]
        public float TriggerDistance
        {
            get { return GetValue<float>(TriggerDistanceProperty); }
            set { SetValue(TriggerDistanceProperty, value); }
        }

        public static readonly PropertyData TriggerDistanceProperty = RegisterProperty("TriggerDistance", typeof(float), () => 0);

        public ObservableCollection<DialoguePrompt> Prompts
        {
            get { return GetValue<ObservableCollection<DialoguePrompt>>(PromptsProperty);  }
            set { SetValue(PromptsProperty, value); }
        }

        public static readonly PropertyData PromptsProperty = RegisterProperty("Prompts", typeof(ObservableCollection<DialoguePrompt>), () => new ObservableCollection<DialoguePrompt>());

        public ObservableCollection<DialoguePrompt> InitialPrompts
        {
            get { return GetValue<ObservableCollection<DialoguePrompt>>(InitialPromptsProperty); }
            set { SetValue(InitialPromptsProperty, value); }
        }

        public static readonly PropertyData InitialPromptsProperty = RegisterProperty("InitialPrompts", typeof(ObservableCollection<DialoguePrompt>), () => new ObservableCollection<DialoguePrompt>());

        public ObservableCollection<DialogueResponse> Responses
        {
            get { return GetValue<ObservableCollection<DialogueResponse>>(ResponsesProperty); }
            set { SetValue(ResponsesProperty, value); }
        }

        public static readonly PropertyData ResponsesProperty = RegisterProperty("Responses", typeof(ObservableCollection<DialogueResponse>), () => new ObservableCollection<DialogueResponse>());

        /// <summary>
        /// The string representation of the dialogue
        /// </summary>
        /// <returns>The conversation's title</returns>
        public override string ToString()
        {
            return Title;
        }

        public static event EventHandler<UICompletedEventArgs> DialogueAdded;

       
        public bool IsCanceled
        {
            get; 
            set;
        }

        [STAThread]
        public static async Task CreateAsync(IUIVisualizerService _uiVisualizerService, MasterViewModel viewModel)
        {

            _ = viewModel.Dispatcher.Invoke(async () =>
              {
                  Dialogue dialogue_new = new Dialogue();
                  ITypeFactory dialogueFactory = viewModel.GetTypeFactory();
                  var dialogVM = dialogueFactory.CreateInstanceWithParametersAndAutoCompletion<DialogueViewModel>(dialogue_new);
                  DialogueAdded += (object sender, UICompletedEventArgs e) =>
                  {
                      if (!dialogue_new.IsCanceled)
                      {
                          Console.WriteLine("[RodskaNote]: New Dialogue - " + dialogue_new.Title);
                          Dialogues.Add(dialogue_new);
                          App app = (App)App.Current;
                          MainWindow window = (MainWindow)app.MainWindow;
                          window.CurrentDocument = dialogue_new;
                      }
                  };
                  var result = await _uiVisualizerService.ShowAsync(dialogVM,DialogueAdded) ?? false;
                  DialogueAdded -= (object sender, UICompletedEventArgs e) =>
                  {
                      if (!dialogue_new.IsCanceled)
                      {
                          Console.WriteLine("[RodskaNote]: New Dialogue - " + dialogue_new.Title);
                          Dialogues.Add(dialogue_new);
                          App app = (App)App.Current;
                          MainWindow window = (MainWindow)app.MainWindow;
                          window.CurrentDocument = dialogue_new;
                      }
                  };
              });
                

            await Task.CompletedTask;
        }


        public static new void InitializeDocumentType(IUIVisualizerService uiVisualizerService, IViewModelLocator viewModelLocator)
        {

            viewModelLocator.Register<DialogueView, DialogueViewModel>();
            viewModelLocator.Register<DialogueControl, DialogueViewModel>();

            uiVisualizerService.Register(typeof(DialogueViewModel), typeof(DialogueView), true);
            Splat.Locator.CurrentMutable.Register(() => new NodeView(), typeof(IViewFor<DialogueNode>));

        }

        public override void Compile()
        {
            StringBuilder nodes = new StringBuilder("local DialogueNodes = {\n");
            nodes.Append("\tPrompts = {\n");
            Prompts.Sort((p1, p2) =>
            {
                if (p1.Priority > p2.Priority)
                {
                    return -1;
                }
                else if (p1.Priority < p2.Priority)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            });
            foreach (DialoguePrompt prompt in Prompts)
            {
                nodes.Append($"\t\t[\"{prompt.Title}\"] = {{\n");
                nodes.Append($"\t\t\tSpeech = \"{prompt.Speech}\";\n");
                nodes.Append($"\t\t\tPrompts = {{\n");
                prompt.ChainedPrompts.Sort((p1, p2) =>
                {
                    if (p1.Priority > p2.Priority)
                    {
                        return -1;
                    }
                    else if (p1.Priority < p2.Priority)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                });
                foreach (DialoguePrompt prompt1 in prompt.ChainedPrompts)
                {
                    nodes.Append($"\t\t\t\t\"{prompt1.Title}\";\n");
                }
                nodes.Append($"\t\t\t}};\n");
                prompt.Responses.Sort((p1, p2) =>
                {
                    if (p1.Order > p2.Order)
                    {
                        return -1;
                    }
                    else if (p1.Order < p2.Order)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                });
                nodes.Append($"\t\t\tResponses = {{\n");
                prompt.Responses.Sort((p1, p2) =>
                {
                    if (p1.Order > p2.Order)
                    {
                        return -1;
                    }
                    else if (p1.Order < p2.Order)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                });
                foreach (DialogueResponse response in prompt.Responses)
                {
                    nodes.Append($"\t\t\t\t\"{response.Title}\";\n");
                }
                nodes.Append($"\t\t\t}};\n");
                nodes.Append($"\t\t\tOverrides = {{}};\n");
                nodes.Append($"\t\t\tCondition = function(player, dialogue)\n");
                nodes.Append($"\t\t\t\t{prompt.ConditionLua}\n");
                nodes.Append($"\t\t\tend;\n");
                nodes.Append($"\t\t\tAction = function(player, dialogue, api)\n");
                nodes.Append($"\t\t\t\t{prompt.ActionLua}\n");
                nodes.Append($"\t\t\tend;\n");
                nodes.Append($"\t\t}};\n");
            }
            nodes.Append("\t};\n");
            nodes.Append("\tResponses = {\n");
            Responses.Sort((p1, p2) =>
            {
                if (p1.Order > p2.Order)
                {
                    return -1;
                }
                else if (p1.Order < p2.Order)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            });
            foreach (DialogueResponse response in Responses)
            {
                nodes.Append($"\t\t[\"{response.Title}\"] = {{\n");
                nodes.Append($"\t\t\tSpeech = \"{response.Speech}\";\n");
                nodes.Append($"\t\t\tPrompts = {{\n");
                response.Prompts.Sort((p1, p2) =>
                {
                    if (p1.Priority > p2.Priority)
                    {
                        return -1;
                    }
                    else if (p1.Priority < p2.Priority)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }

                });
                foreach (DialoguePrompt prompt1 in response.Prompts)
                {
                    nodes.Append($"\t\t\t\t\"{prompt1.Title}\";\n");
                }
                nodes.Append($"\t\t\t}};\n");
                nodes.Append($"\t\t\tOverrides = {{}};\n");
                nodes.Append($"\t\t\tCondition = function(player, dialogue)\n");
                nodes.Append($"\t\t\t\t{response.ConditionLua}\n");
                nodes.Append($"\t\t\tend;\n");
                nodes.Append($"\t\t\tAction = function(player, dialogue, api)\n");
                nodes.Append($"\t\t\t\t{response.ActionLua}\n");
                nodes.Append($"\t\t\tend;\n");
                nodes.Append($"\t\t}};\n");

            }
            nodes.Append($"\t}};\n");
            nodes.Append($"}};\n");
            nodes.Append("\n");
            nodes.Append($"return {{\n");
            nodes.Append($"\tNodes = DialogueNodes;\n");
            nodes.Append($"\tInitialPrompts = {{\n");
            InitialPrompts.Sort((p1, p2) =>
            {
                if (p1.Priority > p2.Priority)
                {
                    return -1;
                }
                else if (p1.Priority < p2.Priority)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            });
            foreach (DialoguePrompt prompt1 in InitialPrompts)
            {
                nodes.Append($"\t\t\"{prompt1.Title}\"\n");
            }
            nodes.Append($"\t}};\n");
            nodes.Append($"\tConversationDistance = {ConversationDistance};\n");
            nodes.Append($"\tTriggerDistance = {TriggerDistance};\n");
            nodes.Append($"\tTriggerOffset = Vector3.new(0,0,0);\n");
            nodes.Append($"\tOverrides = {{\n");
            nodes.Append($"\t\tTitle = (\"{Title}\"):upper();\n");
            nodes.Append($"\t}};\n");
            nodes.Append($"}};");
            CompilationResult = nodes.ToString();
        }

        public static new void PopulateEditor(Dictionary<string,Dictionary<string,object>> details)
        {
            NodeListViewModel listViewModel =  details["Invoker"]["Nodes"] as NodeListViewModel;
        }
        public static new void CreateEditor(LayoutDocument document)
        {
            App app = (App)App.Current;
            document.Content = null;
            ITypeFactory typeFactory = app.currentMainVM.GetTypeFactory();
            MainWindow mainWindow = (MainWindow)app.MainWindow;
            if (mainWindow.CurrentDocument == null)
            {
                return;
            }
            var dialogVM = typeFactory.CreateInstanceWithParametersAndAutoCompletion<DialogueViewModel>((Dialogue)mainWindow.CurrentDocument);
            DialogueControl control = new DialogueControl(dialogVM);
            control.conversationWorkspace.ViewModel = new NodeNetwork.ViewModels.NetworkViewModel();
            DialogueNode node = new DialogueNode(dialogVM.Dialogue);
            node.Name = dialogVM.Dialogue.Title;
            control.conversationWorkspace.ViewModel.Nodes.Add(node);
            control.CurrentDocument = dialogVM.Dialogue;
            document.Content = control;

        }
    }
}
