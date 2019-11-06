using Catel.MVVM;
using RodskaNote.App.Models;
using RodskaNote.Models;
using RodskaNote.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RodskaNote.Views
{
    /// <summary>
    /// Interaction logic for DialogueView.xaml
    /// </summary>
    public partial class InteractionView : Catel.Windows.Window
    {
        private InteractionViewModel vm;

        public InteractionView(IViewModel viewModel)
        {
            vm = (InteractionViewModel)viewModel;
            InitializeComponent();
        }

        private void InteractionTitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = e.Source as TextBox;
            vm.Interaction.Title = box.Text;
        }


        private void Wizard_Cancel(object sender, RoutedEventArgs e)
        {
            vm.Interaction.IsCanceled = true;
        }

        private void ContextControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InteractionContext context = (InteractionContext)e.AddedItems[0];
            vm.Interaction.Context = context;
        }

        private void InContextControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActionType context = (ActionType)e.AddedItems[0];
            vm.Interaction.InputContext = context;
        }
    }
}
