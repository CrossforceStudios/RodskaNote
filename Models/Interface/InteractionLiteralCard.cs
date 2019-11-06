using AdaptiveCards;
using RodskaNote.Models;
using RodskaNote.Presence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.App.Models.Interface
{
    public class InteractionLiteralCard : IWorldDocumentPresence
    {
        public FileStream ActionInput { get; set; }
        public StreamReader InputProcessor { get; set; }

        public string FileName { get; set; }
        public AdaptiveCard GenerateAdaptiveCard(WorldDocument document)
        {
            AdaptiveCard card = this.CreateBaselineCard(document);
            return card;
        }

    }
}
