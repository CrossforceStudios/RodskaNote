using Catel.MVVM;
using Catel.MVVM.Providers;
using Catel.MVVM.Views;
using Catel.Windows;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace RodskaNote.App.Controls
{
    public class RodskaWindow :  RibbonWindow, IDataWindow
    {
        private readonly WindowLogic _logic;

        private event EventHandler<EventArgs> EViewLoaded;
        private event EventHandler<EventArgs> EViewUnloaded;
        private event EventHandler<Catel.MVVM.Views.DataContextChangedEventArgs> EViewDataContextChanged;

        public RodskaWindow()
            : this(null)
        {

        }
        public RodskaWindow(IViewModel viewModel)
        {
            _logic = new WindowLogic(this, null, viewModel);
            _logic.ViewModelChanged += (sender, e) => ViewModelChanged.Invoke(this, e);
            _logic.PropertyChanged += (sender, e) => PropertyChanged.Invoke(this, e);

            Loaded += (sender, e) => EViewLoaded.Invoke(this, e);
            Unloaded += (sender, e) => EViewUnloaded.Invoke(this, e);
            this.AddDataContextChangedHandler((sender, e) => EViewDataContextChanged.Invoke(this, new Catel.MVVM.Views.DataContextChangedEventArgs(e.OldValue, e.NewValue)));

            // Because the RadWindow does not close when DialogResult is set, the following code is required
            ViewModelChanged += (sender, e) => OnViewModelChanged();

            // Call manually the first time (for injected view models)
            OnViewModelChanged();
            
        }

        
        public IViewModel ViewModel
        {
            get { return _logic.ViewModel; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<EventArgs> ViewModelChanged;

        event EventHandler<EventArgs> IView.Loaded
        {
            add { EViewLoaded += value; }
            remove { EViewLoaded -= value; }
        }

        event EventHandler<EventArgs> IView.Unloaded
        {
            add { EViewUnloaded += value; }
            remove { EViewUnloaded -= value; }
        }

        event EventHandler<Catel.MVVM.Views.DataContextChangedEventArgs> IView.DataContextChanged
        {
            add { EViewDataContextChanged += value; }
            remove { EViewDataContextChanged -= value; }
        }

        private void OnViewModelChanged()
        {
            if (ViewModel != null && !ViewModel.IsClosed)
            {
                ViewModel.ClosedAsync += ViewModelClosed;
            }
        }

        private Task ViewModelClosed(object sender, ViewModelClosedEventArgs e)
        {
            Close();
            return Task.CompletedTask;
        }

    }
}