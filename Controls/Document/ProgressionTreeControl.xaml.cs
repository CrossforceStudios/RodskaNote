using Catel.Windows.Controls;
using RodskaNote.App.Models;
using RodskaNote.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RodskaNote.App.Controls.Document
{
    /// <summary>
    /// Interaction logic for ProgressionTreeControl.xaml
    /// </summary>
    public partial class ProgressionTreeControl : UserControl
    {
        public ProgressionTreeControl(ProgressTreeViewModel viewModel): base(viewModel)
        {
            InitializeComponent();
            navigation.SelectedItem = viewModel.Progression.RootNodeP.Children[0];
        }

       
        private void CanAddRegularItem(object sender, CanExecuteRoutedEventArgs e)
        {
            ProgressionEntry entry = navigation.SelectedItem as ProgressionEntry;
            if (entry != null)
            {
                e.CanExecute = entry.ItemType == ProgressionTreeItemType.SpecialBatch;
            }
            else {
                e.CanExecute = false;
            }
            
        }

        private void CanAddRegularBatch(object sender, CanExecuteRoutedEventArgs e)
        {
            ProgressionEntry entry = navigation.SelectedItem as ProgressionEntry;
            if (entry != null)
            {
                e.CanExecute = entry.ItemType == ProgressionTreeItemType.Root;
            }
            else
            {
                e.CanExecute = false;
            }

        }

        private void CanAddSpecialBatch(object sender, CanExecuteRoutedEventArgs e)
        {
            ProgressionEntry entry = navigation.SelectedItem as ProgressionEntry;
            if (entry != null)
            {
                e.CanExecute = entry.ItemType == ProgressionTreeItemType.FullBatch;
            }
            else
            {
                e.CanExecute = false;
            }

        }
        private void AddRegularItem(object sender, ExecutedRoutedEventArgs e)
        {
            ProgressionEntry entry = navigation.SelectedItem as ProgressionEntry;
            entry.Children.Add(new ProgressionEntry()
            {
                ItemType = ProgressionTreeItemType.RegularItem
            });
        }

        private void AddRegularBatch(object sender, ExecutedRoutedEventArgs e)
        {
            ProgressionEntry entry = navigation.SelectedItem as ProgressionEntry;
            entry.Children.Add(new ProgressionEntry()
            {
                ItemType = ProgressionTreeItemType.FullBatch
            });
        }

        private void AddSpecialBatchCmd(object sender, ExecutedRoutedEventArgs e)
        {
            ProgressionEntry entry = navigation.SelectedItem as ProgressionEntry;
            entry.Children.Add(new ProgressionEntry()
            {
                ItemType = ProgressionTreeItemType.SpecialBatch
            });
        }
    }
}
