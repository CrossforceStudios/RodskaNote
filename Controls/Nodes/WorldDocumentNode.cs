using DynamicData;
using NodeNetwork.Toolkit.ValueNode;
using NodeNetwork.ViewModels;
using RodskaNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Controls.Nodes
{
    /// <summary>
    /// Node class used for creating document-based nodes.
    /// </summary>
    /// <typeparam name="T">The <see cref="WorldDocument"/> visualized by the node.</typeparam>
    public abstract class WorldDocumentNode<T>: NodeViewModel where T: WorldDocument
    {
        public WorldDocument InputDocument;
        
       public WorldDocumentNode(string documentNodeTitle, WorldDocument value)
       {
            Name = $"{documentNodeTitle} (GRAPH)";

            InputDocument = value;


       }


       
    }
}
