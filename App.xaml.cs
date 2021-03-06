﻿using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;
using Catel.Services;
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

namespace RodskaNote.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class RodskaApp : RodskaApplication
    {
        private RodskaLoader loader;
        public RodskaApp() : base()
        {
            serviceLocator.RegisterType<IWorldDocumentService, DialogueService>();

        }


         public List<Type> GetLoadedDocumentTypes()
        {
            List<Type> types = new List<Type>();
            foreach (WorldDocument d in loader.WorldDocuments)
            {
                types.Add(d.GetType());
            }
            return types;
        }

        
        
        public void LoadDependencies()
        {
            List<Type> types = GetLoadedDocumentTypes();
            foreach(Type type in types)
            {
                _ = type.GetMethod("InitializeDocumentType", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                        .Invoke(null, new object[] { uiVisualizerService, viewModelLocator });
            }
            List<TreeEntry> treeEntries = loader.TreeEntries;
            foreach(TreeEntry entry in treeEntries)
            {
                TreeEntryTypes.Add(entry.GetType());
            }
            InteractionModels = new ObservableCollection<CreativeDocumentRepresentation>();
            UtilityModels = new ObservableCollection<CreativeDocumentRepresentation>();
            ProgressionModels = new ObservableCollection<CreativeDocumentRepresentation>();

            ObservableCollection<CreativeDocumentRepresentation> _InteractionModels = CreationProvider.InstallDocumentTypes(DocumentUsage.Interaction, types);
            ObservableCollection<CreativeDocumentRepresentation> _UtilityModels = CreationProvider.InstallDocumentTypes(DocumentUsage.Utility, types);
            ObservableCollection<CreativeDocumentRepresentation> _ProgressionModels = CreationProvider.InstallDocumentTypes(DocumentUsage.Progression, types);

            foreach (CreativeDocumentRepresentation rep in _InteractionModels)
            {
                InteractionModels.Add(rep);
            }
            foreach (CreativeDocumentRepresentation rep in _UtilityModels)
            {
                UtilityModels.Add(rep);
            }
            foreach (CreativeDocumentRepresentation rep in _ProgressionModels)
            {
                ProgressionModels.Add(rep);
            }
        }
        private void Rodska_Startup(object sender, StartupEventArgs e)
        {
            Console.WriteLine("Starting RodskaNote...");

            uiVisualizerService = serviceLocator.ResolveType<IUIVisualizerService>();
            viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
            viewModelLocator.Register<MainWindow, MasterViewModel>();

            uiVisualizerService.Register(typeof(MasterViewModel), typeof(MainWindow), true);
            loader = new RodskaLoader();
            loader.Load();
            LoadDependencies();

        }

        private void Rodska_Exit(object sender, ExitEventArgs e)
        {
            loader.Unload();
        }

       
    }
}
