using RodskaNote.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Providers
{
    /// <summary>
    /// Allows for the population of creative controls (controls found in the "Create" section of the ribbon).
    /// </summary>
    public static class CreationProvider
    {

        public static ObservableCollection<CreativeDocumentRepresentation> InstallDocumentTypes(List<Type>? types)
        {
            ObservableCollection<CreativeDocumentModel> initial = new ObservableCollection<CreativeDocumentModel>(CreativeDocumentModel.GetDocumentControlInfo(types));
            ObservableCollection<CreativeDocumentRepresentation> final = new ObservableCollection<CreativeDocumentRepresentation>();
            foreach(CreativeDocumentModel documentModel in initial)
            {
                final.Add(documentModel.ToDocumentRep());
            }
            return final;
        }

        public static ObservableCollection<CreativeDocumentRepresentation> InstallDocumentTypes(DocumentUsage filter, List<Type>? types)
        {
            ObservableCollection<CreativeDocumentModel> initial = new ObservableCollection<CreativeDocumentModel>(CreativeDocumentModel.GetDocumentControlInfo(types));
            ObservableCollection<CreativeDocumentRepresentation> final = new ObservableCollection<CreativeDocumentRepresentation>();
            foreach (CreativeDocumentModel documentModel in initial)
            {
                final.Add(documentModel.ToDocumentRep());
            }
            foreach (CreativeDocumentRepresentation docModel in final)
            {   
                if (docModel.Usage != filter)
                {
                    final.Remove(docModel);
                }
            }
            return final;
        }
    }
}
