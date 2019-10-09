using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;
using Catel.Services;
using Fluent;
using NodeNetwork.ViewModels;
using RodskaNote.Attributes;
using RodskaNote.Models;
using RodskaNote.Providers;
using RodskaNote.Services;
using RodskaNote.Services.Interfaces;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Annotations;

namespace RodskaNote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string Version { get; set; } = "Version 0.1";
        public ObservableCollection<CreativeDocumentRepresentation> InteractionModels { get; set; } = new ObservableCollection<CreativeDocumentRepresentation>();
        public IServiceLocator serviceLocator;
        public IUIVisualizerService uiVisualizerService;
        private IViewModelLocator viewModelLocator;
        public MasterViewModel currentMainVM;
        public List<Type> nodeTypes = new List<Type>();
        public App()
        {
            serviceLocator = ServiceLocator.Default;
            serviceLocator.RegisterType<IWorldDocumentService, DialogueService>();

        }


     
        public void LoadDependencies()
        {
            foreach(Type type in CreativeDocumentModel.GetDocumentTypes<WorldDocument>())
            {
                _ = type.GetMethod("InitializeDocumentType", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).Invoke(null, new object[] { uiVisualizerService, viewModelLocator });
            }
            InteractionModels = new ObservableCollection<CreativeDocumentRepresentation>();
            ObservableCollection<CreativeDocumentRepresentation> _InteractionModels = CreationProvider.InstallDocumentTypes();
            foreach (CreativeDocumentRepresentation rep in _InteractionModels)
            {
                InteractionModels.Add(rep);
            }
        }
        private void Rodska_Startup(object sender, StartupEventArgs e)
        {
            Console.WriteLine("Starting RodskaNote...");

            uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
            viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
            viewModelLocator.Register<MainWindow, MasterViewModel>();

            uiVisualizerService.Register(typeof(MasterViewModel), typeof(MainWindow), true);
            LoadDependencies();

        }

    }
}
