﻿using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using NodeNetwork.Views;
using ReactiveUI;
using RodskaNote.App;
using RodskaNote.Controls.Nodes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Models
{
    [Export(typeof(WorldDocument))]
    public class DialogueResponse: DialogueLeaf
    {

        public ObservableCollection<DialoguePrompt> Prompts
        {
            get { return GetValue<ObservableCollection<DialoguePrompt>>(PromptsProperty); }
            set { SetValue(PromptsProperty, value); }
        }

        public static readonly PropertyData PromptsProperty = RegisterProperty("Prompts", typeof(ObservableCollection<DialoguePrompt>), () => new ObservableCollection<DialoguePrompt>());

        public int Order
        {
            get { return GetValue<int>(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        public static readonly PropertyData OrderProperty = RegisterProperty("Order", typeof(int), () => 0);

        public override void SaveDocumentAs(ITypeFactory factory)
        {
           
        }
        public static new void PopulateEditor(Dictionary<string, Dictionary<string, object>> details)
        {
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            MainWindow window = (MainWindow)app.MainWindow;
            window.ViewModel.ListViewModel.AddNodeType<DialogueResponseNode>(() => new DialogueResponseNode());

        }


        public static new void InitializeDocumentType(IUIVisualizerService uiVisualizerService, IViewModelLocator viewModelLocator)
        {
            Splat.Locator.CurrentMutable.Register(() => new DialogueResponseNodeView(), typeof(IViewFor<DialogueResponseNode>));

        }

        public override void Compile()
        {
            
        }

        public static DialogueResponse GetLeafWithName(string name, ICollection<DialogueResponse> leaves)
        {
            foreach (DialogueResponse leaf1 in leaves)
            {
                if (name == leaf1.Title)
                {
                    return leaf1;
                }
            }
            return new DialogueResponse();
        }
    }

}
