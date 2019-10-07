using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
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
    class DialogueResponseNode: WorldDocumentNode<DialogueResponse>
    {
        public ValueNodeInputViewModel<string> SpeechInput { get; set; }
        public ValueListNodeInputViewModel<DialoguePrompt> PromptsInput { get; set; }

        public ValueNodeInputViewModel<int> Order { get; set; }

        public ValueNodeInputViewModel<string> ConditionLua { get; set; }

        public ValueNodeInputViewModel<string> ActionLua { get; set; }

        public ValueNodeInputViewModel<string> Title { get; set; }

        public ValueNodeOutputViewModel<DialogueResponse> FinalPrompt { get; set; }
        public DialogueResponseNode() : base("Response", null)
        {
            SpeechInput = new ValueNodeInputViewModel<string>()
            {
                Name = "Line of Speech"
            };

            PromptsInput = new ValueListNodeInputViewModel<DialoguePrompt>()
            {
                Name = "Prompts"
            };

            Order = new ValueNodeInputViewModel<int>()
            {
                Name = "Response Order"
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

            FinalPrompt = new ValueNodeOutputViewModel<DialogueResponse>(); 

            this.Inputs.Add(SpeechInput);
            this.Inputs.Add(PromptsInput);
            this.Inputs.Add(Order);
            this.Inputs.Add(ConditionLua);
            this.Inputs.Add(ActionLua);
            this.Inputs.Add(Title);
            this.Outputs.Add(FinalPrompt);
            this.WhenAnyObservable(vm => vm.SpeechInput.Changed, vm => vm.Title.Changed, vm => vm.PromptsInput.Changed, vm => vm.Order.Changed, vm => vm.ActionLua.Changed, vm => vm.ConditionLua.Changed).Subscribe(ip =>
            {

                FinalPrompt.Value = Observable.Return(new DialogueResponse()
                {
                    Speech = SpeechInput.Value,
                    Title = Title.Value,
                    Prompts = new ObservableCollection<DialoguePrompt>(PromptsInput.Values.Items.ToList()),
                    Order = Order.Value,
                    ActionLua = ActionLua.Value,
                    ConditionLua = ConditionLua.Value,
                });
                App app = (App)App.Current;
                MainWindow window = (MainWindow)app.MainWindow;
                window.SignalDocumentChanged(InputDocument);
            });
        }


    }
}
