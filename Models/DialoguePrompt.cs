﻿using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using Catel.Services;
using NodeNetwork.Toolkit.NodeList;
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
    public class DialoguePrompt: DialogueLeaf
    {
        public ObservableCollection<DialoguePrompt> ChainedPrompts
        {
            get { return GetValue < ObservableCollection<DialoguePrompt>>(ChainedPromptsProperty); }
            set {
                ObservableCollection<DialoguePrompt> _oldValue = GetValue<ObservableCollection<DialoguePrompt>>(ChainedPromptsProperty);
                ObservableCollection<DialoguePrompt> _newValue = value;
                TrackSet(ChainedPromptsProperty, _oldValue, _newValue);
            }
        }

        public override void SaveDocumentAs(ITypeFactory factory)
        {

        }

        public static readonly PropertyData ChainedPromptsProperty = RegisterProperty("ChainedPrompts", typeof(ObservableCollection<DialoguePrompt>), () => new ObservableCollection<DialoguePrompt>());

        public ObservableCollection<DialogueResponse> Responses
        {
            get { return GetValue<ObservableCollection<DialogueResponse>>(ResponsesProperty); }
            set {
                ObservableCollection<DialogueResponse> _oldValue = GetValue<ObservableCollection<DialogueResponse>>(ResponsesProperty);
                ObservableCollection<DialogueResponse> _newValue = value;
                TrackSet(ResponsesProperty, _oldValue, _newValue);
            }
        }

        public static readonly PropertyData ResponsesProperty = RegisterProperty("Responses", typeof(ObservableCollection<DialogueResponse>), () => new ObservableCollection<DialogueResponse>());

        public int Priority
        {
            get { return GetValue<int>(PriorityProperty); }
            set {
                int _oldValue = GetValue<int>(PriorityProperty);
                int _newValue = value;
                TrackSet(PriorityProperty, _oldValue, _newValue);
            }
        }

        public static readonly PropertyData PriorityProperty = RegisterProperty("Priority", typeof(int), () => 0);

        public static new void PopulateEditor(Dictionary<string, Dictionary<string, object>> details)
        {
            RodskaApp app = (RodskaApp)RodskaApp.Current;
            MainWindow window = (MainWindow)app.MainWindow;
            window.ViewModel.ListViewModel.AddNodeType<DialoguePromptNode>(() => new DialoguePromptNode());

        }


        public static new void InitializeDocumentType(IUIVisualizerService uiVisualizerService, IViewModelLocator viewModelLocator)
        {
            Splat.Locator.CurrentMutable.Register(() => new DialoguePromptNodeView(), typeof(IViewFor<DialoguePromptNode>));

        }

        public override void Compile()
        {
            
        }

        public static new string GetTypeString()
        {
            return "Dialogue Prompt";
        }


        public static new DialoguePrompt Convert(object obj)
        {
            return (DialoguePrompt)obj;
        }

        public static DialoguePrompt GetLeafWithName(string name, ICollection<DialoguePrompt> leaves)
        {
            foreach (DialoguePrompt leaf1 in leaves)
            {
                if (name == leaf1.Title)
                {
                    return leaf1;
                }
            }
            return new DialoguePrompt();
        }

    }
}
