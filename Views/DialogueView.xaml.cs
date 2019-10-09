using Catel.MVVM;
using FontAwesome.WPF;
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
    public partial class DialogueView : Catel.Windows.Window
    {
        private DialogueViewModel vm;

        public FontAwesomeIcon DialogueIcon { get; set; } = FontAwesomeIcon.Comment;
        public DialogueView(IViewModel viewModel)
        {
            vm = (DialogueViewModel)viewModel;
            InitializeComponent();
        }

        private void DialogueTitleBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = e.Source as TextBox;
            vm.Dialogue.Title = box.Text;
        }

        private void ConvoDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float newDistance = (float)e.NewValue;
            vm.Dialogue.ConversationDistance = newDistance;
        }

        private void TriggerDistanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float newDistance = (float)e.NewValue;
            vm.Dialogue.TriggerDistance = newDistance;
        }

        private void Wizard_Cancel(object sender, RoutedEventArgs e)
        {
            vm.Dialogue.IsCanceled = true;
        }
    }
}
