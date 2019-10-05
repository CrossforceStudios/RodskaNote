using Catel.Data;
using Catel.IoC;
using Catel.Services;
using RodskaNote.Attributes;
using RodskaNote.Models;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

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


    }
}
