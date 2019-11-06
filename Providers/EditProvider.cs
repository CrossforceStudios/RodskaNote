using RodskaNote.Attributes;
using RodskaNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xceed.Wpf.Toolkit.PropertyGrid;


namespace RodskaNote.Providers
{
    /// <summary>
    /// Class used to set up editing tools in the <see cref="MainWindow"/>
    /// </summary>
    public static class EditProvider
    {
        /// <summary>
        /// Retrieves a collection of property metadata (<see cref="PropertyDefinition"/>) for use with the properties explorer.
        /// </summary>
        /// <typeparam name="T">A world design document type (<see cref="WorldDocument"/>) that is used for setting up the properties explorer</typeparam>
        /// <returns>A collection of property metadata besides the title that is to be used with the properties explorer.</returns>
        public static List<string> GetUnavailableProperties<T>() where T: WorldDocument{
            Type type = typeof(T);
            List<string> propertyDefinitions = new List<string>();
            List<PropertyInfo> fields = new List<PropertyInfo>(type.GetProperties(BindingFlags.Public)); 
            foreach(PropertyInfo field in fields)
            {
                PropertiesDisabled properties = (PropertiesDisabled)field.GetCustomAttribute(typeof(PropertiesDisabled));
                if (properties != null)
                {

                    propertyDefinitions.Add(field.Name);
                }
            }
            return propertyDefinitions;
        }
        /// <summary>
        /// Same as <see cref="GetAvailableProperties{T}"/>, but for dynamic use
        /// </summary>
        /// <param name="T">The <see cref="WorldDocument"/> type given for property indexing</param>
        /// <returns>The same result as <see cref="GetAvailableProperties{T}"/></returns>
        public static List<string> GetUnavailableProperties(Type T)
        {
            Type type = T;
            List<string> propertyDefinitions = new List<string>();
            PropertyInfo[] fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < fields.Length; i++)
            {
                PropertyInfo field = fields[i];
                PropertyDefinition property = new PropertyDefinition();
                PropertiesDisabled properties = (PropertiesDisabled)Attribute.GetCustomAttribute(field,typeof(PropertiesDisabled));
                if (properties != null)
                {
                    
                    propertyDefinitions.Add(field.Name);
                }
            }
            return propertyDefinitions;
        }
    }
}
