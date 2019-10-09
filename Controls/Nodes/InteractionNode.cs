using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using ReactiveUI;
using RodskaNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Controls.Nodes
{
    class InteractionNode : WorldDocumentNode<InteractionLiteral>
    {
        public ValueNodeInputViewModel<string> Description { get; set; }

        public ValueNodeInputViewModel<string> ActionBuilderLua { get; set; }

        public ValueNodeInputViewModel<string> CreationCondition { get; set; }

        public ValueNodeInputViewModel<float> ActivationDistance { get; set; }

        public ValueNodeInputViewModel<string> ClientInt { get; set; }

        public ValueNodeInputViewModel<string> Location { get; set; }

        public ValueNodeOutputViewModel<InteractionLiteral> Output { get; set; }

        public InteractionNode(InteractionLiteral value) : base("Interaction", value)
        {
            Description = new ValueNodeInputViewModel<string>()
            {
                Name = "Description"
            };

            ActionBuilderLua = new ValueNodeInputViewModel<string>()
            {
                Name = "Action Builder (Lua Source)"
            };

            CreationCondition = new ValueNodeInputViewModel<string>()
            {
                Name = "Creation Condition (Lua Source)"
            };

            ActivationDistance = new ValueNodeInputViewModel<float>()
            {
                Name = "Activation Distance"
            };

            ClientInt = new ValueNodeInputViewModel<string>()
            {
                Name = "Client Interaction Code (Lua Source)"
            };

            Location = new ValueNodeInputViewModel<string>()
            {
                Name = "Search Location (Lua Source)"
            };

            Output = new ValueNodeOutputViewModel<InteractionLiteral>()
            {
                Name = "Resulting Interaction"
            };
            this.Inputs.Add(Description);
            this.Inputs.Add(ActionBuilderLua);
            this.Inputs.Add(CreationCondition);
            this.Inputs.Add(ActivationDistance);
            this.Inputs.Add(ClientInt);
            this.Inputs.Add(Location);
            this.Outputs.Add(Output);

            this.WhenAnyObservable(vm => vm.Description.Changed, vm => vm.ActionBuilderLua.Changed, vm => vm.CreationCondition.Changed, vm => vm.ActivationDistance.Changed, vm => vm.ClientInt.Changed, vm => vm.Location.Changed).Subscribe(ip =>
            {
                InteractionLiteral interaction = InputDocument as InteractionLiteral;
                interaction.Description = Description.Value;
                interaction.ActionBuilderLua = ActionBuilderLua.Value;
                interaction.CreationConditionLua = CreationCondition.Value;
                interaction.ActivationDistance = ActivationDistance.Value;
                interaction.ClientIntLua = ClientInt.Value;
                interaction.LocationLua = Location.Value;

                InputDocument = interaction;
                Output.Value = Observable.Return(interaction);
            });
            CanBeRemovedByUser = false;

        }


    }
}
