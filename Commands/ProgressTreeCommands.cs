using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RodskaNote.App.Commands
{
    public static class ProgressTreeCommands
    {
        public static RoutedUICommand AddSpecialBatchCommand = new RoutedUICommand();
        public static RoutedUICommand AddRegularItemCommand = new RoutedUICommand();
        public static RoutedUICommand AddRegularBatchCommand = new RoutedUICommand();
    }
}
