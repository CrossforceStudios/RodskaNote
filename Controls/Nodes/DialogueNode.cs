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
    class DialogueNode: WorldDocumentNode<Dialogue>
    {
        public ValueListNodeInputViewModel<DialoguePrompt> InitialPrompts { get; set; }

        public ValueListNodeInputViewModel<DialoguePrompt> Prompts { get; set; }

        public ValueListNodeInputViewModel<DialogueResponse> Responses { get; set; }

        public ValueNodeOutputViewModel<Dialogue> Output { get; set; }
        public DialogueNode(Dialogue value) : base("Conversation",value)
        {
            InitialPrompts = new ValueListNodeInputViewModel<DialoguePrompt>()
            {
                Name = "Initial Prompts"
            };

            Prompts = new ValueListNodeInputViewModel<DialoguePrompt>()
            {
                Name = "Prompts"
            };

            Responses = new ValueListNodeInputViewModel<DialogueResponse>()
            {
                Name = "Responses"
            };

            Inputs.Add(InitialPrompts);
            Inputs.Add(Prompts);
            Inputs.Add(Responses);


            Output = new ValueNodeOutputViewModel<Dialogue>()
            {
                Name = "Final Result",
                Visibility = NodeNetwork.ViewModels.EndpointVisibility.AlwaysHidden,
            };
            this.WhenAnyObservable(vm => vm.InitialPrompts.Changed, vm => vm.Prompts.Changed, vm => vm.Responses.Changed).Subscribe(ip =>
            {
                Dialogue dialogue = InputDocument as Dialogue;
                dialogue.Prompts = new ObservableCollection<DialoguePrompt>(Prompts.Values.Items.ToList());
                dialogue.Responses = new ObservableCollection<DialogueResponse>(Responses.Values.Items.ToList());
                dialogue.InitialPrompts = new ObservableCollection<DialoguePrompt>(InitialPrompts.Values.Items.ToList());
                InputDocument = dialogue;
                Output.Value = Observable.Return(dialogue);
            });
            App app = (App)App.Current;
            MainWindow window = (MainWindow)app.MainWindow;
            window.DocumentChanged += Window_DocumentChanged;



        }

        private void Window_DocumentChanged(object sender, EventArgs e)
        {

        }
    }

}
