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
using RodskaNote.App.Controls.Document;
using RodskaNote.Controls.Nodes;
using RodskaNote.Models;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Threading;
using Xceed.Wpf.AvalonDock.Layout;
using NodeNetwork.ViewModels;
using RodskaNote.Controls.Nodes.Primitives;
using Catel.Runtime.Serialization.Xml;
using Catel.Runtime.Serialization;
using NodeNetwork.Toolkit.Layout.ForceDirected;
using System.Windows.Shell;
using RodskaNote.Views;
using System.ComponentModel.Composition;
using RodskaNote.App;

namespace RodskaNote
{
    /// <summary>
    /// <see cref="WorldDocument"/> used for storing conversation interactions between a player and an NPC.
    /// </summary>
    [Export(typeof(WorldDocument))]
    [CreativeDocumentModel("Dialogue", "Conversation", "Used to make interactions between the player and an NPC.", DocumentUsage.Interaction, Icon =FontAwesome.WPF.FontAwesomeIcon.Comment)]
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


        public override void SaveDocumentAs(ITypeFactory factory)
        {

            XmlSerializer seri = new XmlSerializer(new SerializationManager(), new DataContractSerializerFactory(), new XmlNamespaceManager(), factory, new ObjectAdapter());
            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "Rodska Note Documents (.rndml)|*.rndml",
                FileName = "Dialogue",
                DefaultExt = ".rndml"
            };
            bool? result  = dlg.ShowDialog();
            if(result == true)
            {
                FileStream stream = new FileStream(dlg.FileName,FileMode.Create);
                seri.Serialize(this, stream);
                stream.Close();
                JumpList jl = JumpList.GetJumpList(RodskaApplication.Current);
                bool jpExisting = false;
                foreach(JumpItem item in jl.JumpItems)
                {
                    if(item is JumpPath)
                    {
                        JumpPath p = item as JumpPath;
                        if(p.Path == dlg.FileName)
                        {
                            jpExisting = true;
                            break;
                        }
                    }
                }
                if(!jpExisting)
                {
                    JumpPath jp = new JumpPath
                    {
                        Path = dlg.FileName,
                        CustomCategory = Type
                    };
                    jl.JumpItems.Add(jp);
                    JumpList.AddToRecentCategory(jp);
                    jl.Apply();
                }
            }
        }
        
        [STAThread]
        public static async Task CreateAsync(IUIVisualizerService _uiVisualizerService, MasterViewModel viewModel)
        {

            _ = viewModel.Dispatcher.Invoke(async () =>
              {
                  Dialogue dialogue_new = new Dialogue();
                  ITypeFactory dialogueFactory = viewModel.GetTypeFactory();
                  var dialogVM = dialogueFactory.CreateInstanceWithParametersAndAutoCompletion<DialogueViewModel>(dialogue_new);
                  DocumentAdded += (object sender, UICompletedEventArgs e) =>
                  {
                      if (!dialogue_new.IsCanceled)
                      {
                          Console.WriteLine("[RodskaNote]: New Dialogue - " + dialogue_new.Title);
                          Dialogues.Add(dialogue_new);
                          RodskaApp app = (RodskaApp)RodskaApp.Current;
                          MainWindow window = (MainWindow)app.MainWindow;
                          window.CurrentDocument = dialogue_new;
                      }
                  };
                  var result = await GetFromPrompt(_uiVisualizerService, dialogVM);
                  DocumentAdded -= (object sender, UICompletedEventArgs e) =>
                  {
                      if (!dialogue_new.IsCanceled)
                      {
                          Console.WriteLine("[RodskaNote]: New Dialogue - " + dialogue_new.Title);
                          Dialogues.Add(dialogue_new);
                          RodskaApplication app = (RodskaApplication)RodskaApplication.Current;
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
            Dialogue dialogue = (Dialogue)mainWindow.CurrentDocument;
            DialogueViewModel dialogVM = typeFactory.CreateInstanceWithParametersAndAutoCompletion<DialogueViewModel>(dialogue);
            DialogueControl control = new DialogueControl(dialogVM);
            control.conversationWorkspace.ViewModel = new NodeNetwork.ViewModels.NetworkViewModel();
            DialogueNode node = new DialogueNode(dialogue);

            using (control.ConversationWorkspace.ViewModel.SuppressChangeNotifications())
            {
                Dictionary<string, DialoguePromptNode> PromptNodes = new Dictionary<string, DialoguePromptNode>();
                Dictionary<string, DialogueResponseNode> ResponseNodes = new Dictionary<string, DialogueResponseNode>();
                
                if (dialogue.Prompts.Count > 0)
                {
                    foreach (DialoguePrompt prompt in dialogue.Prompts)
                    {
                        DialoguePromptNode dialoguePrompt = new DialoguePromptNode();
                        StringNode SpeechNode = new StringNode();
                        SpeechNode.ValueEditor.Value = prompt.Speech;
                        ConnectionViewModel speechToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialoguePrompt.SpeechInput, SpeechNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(SpeechNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(speechToPrompt);

                        StringNode TitleNode = new StringNode();
                        TitleNode.ValueEditor.Value = prompt.Title;
                        ConnectionViewModel titleToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialoguePrompt.Title, TitleNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(TitleNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(titleToPrompt);

                        IntegerNode PriorityNode = new IntegerNode();
                        PriorityNode.ValueEditor.Value = prompt.Priority;
                        ConnectionViewModel priorityToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialoguePrompt.Priority, PriorityNode.Output);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(PriorityNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(priorityToPrompt);

                        StringNode ActionLuaNode = new StringNode();
                        ActionLuaNode.ValueEditor.Value = prompt.ActionLua ?? "";
                        ConnectionViewModel actionLuaToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialoguePrompt.ActionLua, ActionLuaNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(ActionLuaNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(actionLuaToPrompt);

                        StringNode ConditionLuaNode = new StringNode();
                        ActionLuaNode.ValueEditor.Value = prompt.ConditionLua ?? "return true";
                        ConnectionViewModel conditionLuaToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialoguePrompt.ConditionLua, ConditionLuaNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(ConditionLuaNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(conditionLuaToPrompt);

                        ConnectionViewModel promptToDialogue = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, node.Prompts, dialoguePrompt.FinalPrompt);
                        ConnectionViewModel promptToDialogue2;
                        if (DialoguePrompt.GetLeafWithName(prompt.Title, dialogue.InitialPrompts) != null)
                        {
                            promptToDialogue2 = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, node.InitialPrompts, dialoguePrompt.FinalPrompt);
                            control.ConversationWorkspace.ViewModel.Connections.Add(promptToDialogue2);
                        }
                        control.ConversationWorkspace.ViewModel.Nodes.Add(dialoguePrompt);
                        control.ConversationWorkspace.ViewModel.Connections.Add(promptToDialogue);

                        PromptNodes[prompt.Title] = dialoguePrompt;
                    }
                }

                if (dialogue.Responses.Count > 0)
                {
                    Console.WriteLine("Responses found.");
                    foreach (DialogueResponse response in dialogue.Responses)
                    {
                        Console.WriteLine(response);
                        DialogueResponseNode dialogueResponse = new DialogueResponseNode();
                        StringNode SpeechNode = new StringNode();
                        SpeechNode.ValueEditor.Value = response.Speech;
                        ConnectionViewModel speechToResponse = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialogueResponse.SpeechInput, SpeechNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(SpeechNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(speechToResponse);

                        StringNode TitleNode = new StringNode();
                        TitleNode.ValueEditor.Value = response.Title;
                        ConnectionViewModel titleToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialogueResponse.Title, TitleNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(TitleNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(titleToPrompt);

                        IntegerNode OrderNode = new IntegerNode();
                        OrderNode.ValueEditor.Value = response.Order;
                        ConnectionViewModel priorityToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialogueResponse.Order, OrderNode.Output);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(OrderNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(priorityToPrompt);

                        StringNode ActionLuaNode = new StringNode();
                        ActionLuaNode.ValueEditor.Value = response.ActionLua ?? "";
                        ConnectionViewModel actionLuaToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialogueResponse.ActionLua, ActionLuaNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(ActionLuaNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(actionLuaToPrompt);

                        StringNode ConditionLuaNode = new StringNode();
                        ActionLuaNode.ValueEditor.Value = response.ConditionLua ?? "return true";
                        ConnectionViewModel conditionLuaToPrompt = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, dialogueResponse.ConditionLua, ConditionLuaNode.StringOutput);

                        control.ConversationWorkspace.ViewModel.Nodes.Add(ConditionLuaNode);
                        control.ConversationWorkspace.ViewModel.Connections.Add(conditionLuaToPrompt);

                        ConnectionViewModel promptToDialogue = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, node.Responses, dialogueResponse.FinalPrompt);
                        control.ConversationWorkspace.ViewModel.Nodes.Add(dialogueResponse);
                        control.ConversationWorkspace.ViewModel.Connections.Add(promptToDialogue);

                        ResponseNodes[response.Title] = dialogueResponse;

                    }
                }

                foreach(DialoguePrompt prompt in dialogue.Prompts)
                {
                    DialoguePromptNode pNode = PromptNodes[prompt.Title];
                    if (prompt.Responses.Count > 0 && pNode != null)
                    {
                        foreach(DialogueResponse response in prompt.Responses)
                        {
                            if (ResponseNodes[response.Title] != null) {
                                ConnectionViewModel connection = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, pNode.Responses, ResponseNodes[response.Title].FinalPrompt);
                                control.ConversationWorkspace.ViewModel.Connections.Add(connection);
                            }
                        }
                        if (prompt.ChainedPrompts.Count > 0 && pNode != null)
                        {
                            foreach (DialoguePrompt prompt1 in prompt.ChainedPrompts)
                            {
                                if (PromptNodes[prompt.Title] != null)
                                {
                                    ConnectionViewModel connection = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, pNode.ChainedPromptsInput, PromptNodes[prompt.Title].FinalPrompt);
                                    control.ConversationWorkspace.ViewModel.Connections.Add(connection);
                                }
                            }
                        }
                    }
                }

                foreach (DialogueResponse response in dialogue.Responses)
                {
                    DialogueResponseNode rNode = ResponseNodes[response.Title];
                    if (response.Prompts.Count > 0 && rNode != null)
                    {
                        foreach (DialoguePrompt prompt in response.Prompts)
                        {
                            if (PromptNodes[prompt.Title] != null)
                            {
                                ConnectionViewModel connection = new ConnectionViewModel(control.ConversationWorkspace.ViewModel, rNode.PromptsInput, PromptNodes[prompt.Title].FinalPrompt);
                                control.ConversationWorkspace.ViewModel.Connections.Add(connection);
                            }
                        }
                    }
                }
            }
            node.Name = dialogue.Title;
            control.conversationWorkspace.ViewModel.Nodes.Add(node);

            control.CurrentDocument = dialogue;
            document.Content = control;
            
           

        }


        public Dialogue()
        {
            Type = "Dialogue";
            DisplayType = "Conversation";

        }

        public static new string GetTypeString()
        {
            return "Dialogue";
        }
    }
}
