using Catel.Data;
using RodskaNote.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.App.Models
{
    public enum ProgressionTreeItemType
    {
        FullBatch,
        SpecialBatch,
        PosterItem,
        RewardItem,
        RegularItem,
        Root
    }
    [Export(typeof(TreeEntry))]
    public class ProgressionEntry: TreeEntry
    {
        
        public int Level
        {
            get { return GetValue<int>(LevelProperty); }
            set
            {
                var oldVal = GetValue<int>(LevelProperty);
                var newVal = value;
                TrackSet(LevelProperty, oldVal, newVal);
            }
        }

        public static readonly PropertyData LevelProperty = RegisterProperty("Level", typeof(int), null);

        public ProgressionTreeItemType ItemType
        {
            get { return GetValue<ProgressionTreeItemType>(ProgressProperty); }
            set
            {
                var oldVal = GetValue<ProgressionTreeItemType>(ProgressProperty);
                var newVal = value;
                TrackSet(ProgressProperty, oldVal, newVal);
            }
        }

        public static readonly PropertyData ProgressProperty = RegisterProperty("ItemType", typeof(ProgressionTreeItemType), null);

        public ProgressionEntry(): base()
        {
            this.Level = 1;
            this.Children = new ObservableCollection<TreeEntry>();
        }
    }
}
