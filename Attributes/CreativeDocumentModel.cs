using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Catel.Data;
using Catel.Services;
using RodskaNote.Models;
using RodskaNote.ViewModels;

namespace RodskaNote.Attributes
{
    /// <summary>
    /// An enum that describes what type of world design document is created.
    /// </summary>
    public enum DocumentUsage
    {
        /// <summary>
        /// Used when making documents that describe interactions with the world, other characters or players, and the story itself.
        /// </summary>
        Interaction,
        /// <summary>
        /// Used when describing cutscenes and non-interactive dialogue.
        /// </summary>
        Scenes,
        /// <summary>
        /// Used when describing elements such as missions, factions and more.
        /// </summary>
        Progression,
        /// <summary>
        /// Used for campaigns and co-op missions
        /// </summary>
        Storyline
    }
    /// <summary>
    /// An attribute that makes a <see cref="WorldDocument"/> accessible in the Create tab of RodskaNote.
    /// </summary>

    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    public class CreativeDocumentModel : System.Attribute
    {
        /// <summary>
        /// The name of the button and the element being created.
        /// </summary>
        public string Name;
        /// <summary>
        /// The human-readable name of the element being created.
        /// </summary>
        public string Title;
        /// <summary>
        /// Help text that explains what the element is for.
        /// </summary>
        public string Description;
        /// <summary>
        /// Where the element creation button lies in the UI layout.
        /// </summary>
        public DocumentUsage Usage;


        public Func<IUIVisualizerService, MasterViewModel, Task> CreationCommand;

        public Type objectType;
        /// <summary>
        /// Main constructor. Used for the entire document type.
        /// </summary>
        /// <param name="name">The desired name of the button and the element being created.</param>
        /// <param name="title">The desired human-readable name of the element being created.</param>
        /// <param name="description">The desired help text that explains what the element is for.</param>
        /// <param name="usage">Where the element creation button lies in the UI layout (as desired).</param>
        public CreativeDocumentModel(string name, string title, string description, DocumentUsage usage)
        {
            Name = name;
            Title = title;
            Description = description;
            Usage = usage;
        }

        public static List<Type> GetDocumentTypes<T>() where T : WorldDocument
        {
            List<Type> objects = new List<Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(WorldDocument))))
            {
                objects.Add(type);
            }
            objects.Sort();
            return objects;
        }

        /// <summary>
        /// Retrieves all human-readable information on all <see cref="WorldDocument"/> types.
        /// </summary>
        /// <returns>A <see cref="List{T}"/> of all world design metadata (<see cref="CreativeDocumentModel"/>) for use with the UI</returns>
        public static List<CreativeDocumentModel> GetDocumentControlInfo()
        {
            List<CreativeDocumentModel> docAttrs = new List<CreativeDocumentModel>();
            List<Type> documentTypes = GetDocumentTypes<WorldDocument>();
            foreach(Type type in documentTypes)
            {
                CreativeDocumentModel cdm = (CreativeDocumentModel) GetCustomAttribute(type, typeof(CreativeDocumentModel));
                var method = type.GetMethod("CreateAsync", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                Console.WriteLine(method.ToString());
                if (method != null)
                {
                    cdm.CreationCommand = async (IUIVisualizerService vs, MasterViewModel viewModel) =>
                    {
                        await Task.Run(() => method.Invoke(null, new object[] { vs, viewModel }));
                    };
                }
                cdm.objectType = type;
                docAttrs.Add(cdm);
            }
            return docAttrs;
        }



        public CreativeDocumentRepresentation ToDocumentRep()
        {
            CreativeDocumentRepresentation rep = new CreativeDocumentRepresentation
            {
                Name = Name,
                Title = Title,
                Description = Description,
                Usage = Usage
            };
            if (CreationCommand != null)
            {
                rep.CreationCommand = CreationCommand;
            }
            if (objectType != null)
            {
                rep.ObjectType = objectType;
            }
            return rep;
        }
    }


 
}
