namespace RodskaNote.ViewModels
{
    using Catel;
    using Catel.MVVM;
    using Catel.Services;
    using Catel.IoC;
    using RodskaNote.Services.Interfaces;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;
    using Catel.Data;
    using RodskaNote.Attributes;
    using System;
    using System.Windows.Threading;
    using RodskaNote.Models;

    public class MasterViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        public Dispatcher Dispatcher;
        public MasterViewModel(Dispatcher dispatcher)
        {
            App app = (App) App.Current;
            _uiVisualizerService = app.uiVisualizerService;
            app.currentMainVM = this;
            Dispatcher = dispatcher;
            OpenDocuments = new ObservableCollection<WorldDocument>();
        }

        public ObservableCollection<WorldDocument> OpenDocuments
        {
            get; set;
        }

        public WorldDocument CurrentDocument
        {
            get; set;
        }

        /**
         * 
         */


        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
