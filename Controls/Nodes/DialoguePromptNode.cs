using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using RodskaNote.App;
using RodskaNote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Controls.Nodes
{
   public  class DialoguePromptNode: WorldDocumentNode<DialoguePrompt>
    {

        public  ValueNodeInputViewModel<string> SpeechInput { get; set; }
        public  ValueListNodeInputViewModel<DialoguePrompt> ChainedPromptsInput { get; set; }

        public ValueNodeInputViewModel<int> Priority { get; set; }

        public ValueNodeInputViewModel<string> ConditionLua { get; set; }

        public ValueNodeInputViewModel<string> ActionLua { get; set; }

        public ValueNodeInputViewModel<string> Title { get; set; }

        public ValueListNodeInputViewModel<DialogueResponse> Responses { get; set; }
        public ValueNodeOutputViewModel<DialoguePrompt> FinalPrompt { get; set; }
        public DialoguePromptNode() : base("Prompt", null)
        {
            SpeechInput = new ValueNodeInputViewModel<string>()
            {
                Name = "Line of Speech"
            };

            ChainedPromptsInput = new ValueListNodeInputViewModel<DialoguePrompt>()
            {
                Name = "Chained Prompts"
            };

            Priority = new ValueNodeInputViewModel<int>()
            {
                Name = "Prompt Priority"
            };

            ConditionLua = new ValueNodeInputViewModel<string>()
            {
                Name = "Lua Condition"
            };

            ActionLua = new ValueNodeInputViewModel<string>()
            {
                Name = "Lua Action"
            };

            Title = new ValueNodeInputViewModel<string>()
            {
                Name = "Prompt Name"
            };

            Responses = new ValueListNodeInputViewModel<DialogueResponse>()
            {
                Name = "Responses"
            };

            FinalPrompt = new ValueNodeOutputViewModel<DialoguePrompt>()
            {
                Name = "Resulting Prompt"

            };
            this.Inputs.Add(SpeechInput);
            this.Inputs.Add(ChainedPromptsInput);
            this.Inputs.Add(Priority);
            this.Inputs.Add(ConditionLua);
            this.Inputs.Add(ActionLua);
            this.Inputs.Add(Title);
            this.WhenAnyObservable(vm => vm.SpeechInput.Changed, vm => vm.Title.Changed, vm => vm.ChainedPromptsInput.Changed, vm => vm.Priority.Changed, vm => vm.Responses.Changed, vm => vm.ActionLua.Changed, vm => vm.ConditionLua.Changed).Subscribe(ip =>
            {
                 InputDocument = new DialoguePrompt()
                {
                    Speech = SpeechInput.Value,
                    Title = Title.Value,
                    ChainedPrompts = new ObservableCollection<DialoguePrompt>(ChainedPromptsInput.Values.Items.ToList()),
                    Priority = Priority.Value,
                    Responses = new ObservableCollection<DialogueResponse>(Responses.Values.Items.ToList()),
                    ActionLua = ActionLua.Value,
                    ConditionLua = ConditionLua.Value,
                };
                FinalPrompt.Value =  Observable.Return(InputDocument as DialoguePrompt);
                RodskaApp app = (RodskaApp)RodskaApp.Current;
                MainWindow window = (MainWindow)app.MainWindow;
                window.SignalDocumentChanged(FinalPrompt.Value.Wait());
             }); 
            
            this.Inputs.Add(Responses);
            this.Outputs.Add(FinalPrompt);
        }   
     }
 }

