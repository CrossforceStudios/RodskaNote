using Catel;
using Catel.Data;
using Catel.MVVM;
using RodskaNote.App.Models;
using System.Threading.Tasks;

namespace RodskaNote.App.ViewModels
{


    public class ProgressTreeViewModel : ViewModelBase
    {
       
        [Model]
        public ProgressionTree Progression
        {
            get { return GetValue<ProgressionTree>(ProgressionProperty); }
            set { SetValue(ProgressionProperty, value); }
        }
        public static readonly PropertyData ProgressionProperty = RegisterProperty("Progression", typeof(ProgressionTree), null);

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

        public ProgressTreeViewModel(ProgressionTree progression)
        {
            Argument.IsNotNull(() => progression);
            Progression = progression;
        }
    }
}
