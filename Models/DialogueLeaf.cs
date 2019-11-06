using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Models
{
    [Export(typeof(WorldDocument))]
    public abstract class DialogueLeaf : WorldDocument
    {
       public string Name
        {
            get { return Title; }
            set { Title = value;  }
        }

        public string Speech
        {
            get { return GetValue<string>(SpeechProperty); }
            set {
                string _oldValue = GetValue<string>(SpeechProperty);
                string _newValue = value;
                TrackSet(SpeechProperty, _oldValue, _newValue);
            }
        }

        public static readonly PropertyData SpeechProperty = RegisterProperty("Speech", typeof(string), () => "Hello World");
  
        public string LeafTitle
        {
            get { return GetValue<string>(LeafTitleProperty); }
            set {
                string _oldValue = GetValue<string>(LeafTitleProperty);
                string _newValue = value;
                TrackSet(LeafTitleProperty, _oldValue, _newValue);
            }
        }

        public static readonly PropertyData LeafTitleProperty = RegisterProperty("LeafTitle", typeof(string), null);

        public string ConditionLua
        {
            get { return GetValue<string>(ConditionLuaProperty);  }
            set {
                string _oldValue = GetValue<string>(ConditionLuaProperty);
                string _newValue = value;
                TrackSet(ConditionLuaProperty, _oldValue, _newValue);
            }
        }

        public static readonly PropertyData ConditionLuaProperty = RegisterProperty("ConditionLua", typeof(string), () => "return true");

        public string ActionLua
        {
            get { return GetValue<string>(ActionLuaProperty); }
            set {
                string _oldValue = GetValue<string>(ActionLuaProperty);
                string _newValue = value;
                TrackSet(ActionLuaProperty, _oldValue, _newValue);
            }
        }

        public static readonly PropertyData ActionLuaProperty = RegisterProperty("ActionLua", typeof(string), () => "");

        public static new  void InitializeDocumentType(IUIVisualizerService uiVisualizerService, IViewModelLocator viewModelLocator)
        {

        }

        public static DialogueLeaf GetLeafWithName(string name, ICollection<DialogueLeaf> leaves)
        {
            foreach(DialogueLeaf leaf1 in leaves)
            {
                if (name == leaf1.Title)
                {
                    return leaf1;
                }
            }
            return null;
        }

        public static new DialogueLeaf Convert(object obj)
        {
            return (DialogueLeaf)obj;
        }

        public static new string GetTypeString()
        {
            return "Dialogue Leaf";
        }


    }


}
