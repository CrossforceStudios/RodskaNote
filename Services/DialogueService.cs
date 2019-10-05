using Catel;
using Catel.Data;
using Catel.Runtime.Serialization.Xml;
using RodskaNote.Models;
using RodskaNote.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Services
{
    public class DialogueService : IWorldDocumentService

    {
        private readonly string _path;
        private readonly IXmlSerializer _xmlSerializer;

        public IEnumerable<WorldDocument> LoadDocuments()
        {
           if (!File.Exists(_path))
            {
                return new Dialogue[] { };
            }

            using var fileStream = File.Open(_path, FileMode.Open);
            Dialogues dialogues = (Dialogues)_xmlSerializer.Deserialize(typeof(Dialogues), fileStream);
            return dialogues.AllDialogues.ToArray();
        }

        public void SaveDocuments(IEnumerable<WorldDocument> documents)
        {
            Dialogues dialogueHolder = new Dialogues();
            foreach(WorldDocument doc in documents)
            {
                Dialogue dialogue = (Dialogue)doc;
                dialogueHolder.AllDialogues.Add(dialogue);
                using var fileStream = File.Open(_path, FileMode.Create);
                _xmlSerializer.Serialize(dialogueHolder, fileStream);
            }
            
        }

        public DialogueService(IXmlSerializer serializer)
        {
            Argument.IsNotNull(() => serializer);

            _xmlSerializer = serializer;

            string directory = Catel.IO.Path.GetApplicationDataDirectory("Crossforce Studios", "RodskaNote");

            _path = Path.Combine(directory, "dialogues.rndml");

        }


    }
}
