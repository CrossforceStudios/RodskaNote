using RodskaNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Services.Interfaces
{
    /// <summary>
    /// An interface used for the implementation of persistent world design documents of type (<see cref="WorldDocument"/>).
    /// </summary>
    public interface IWorldDocumentService
    {
        /// <summary>
        /// Retrieves a set of <c>WorldDocument</c>s packaged into an <see cref="IEnumerable{WorldDocument}"/>.
        /// </summary>
        /// <returns>An array representation of all of the world design documents saved.</returns>
        IEnumerable<WorldDocument> LoadDocuments();
        /// <summary>
        /// Serializes and stores the given world design documents (<see cref="WorldDocument"/>) into a special XML file for later use.
        /// </summary>
        /// <param name="documents">An array of all the world design documents (<see cref="WorldDocument"/>) that are to be saved.</param>
        void SaveDocuments(IEnumerable<WorldDocument> documents);
    }
}
