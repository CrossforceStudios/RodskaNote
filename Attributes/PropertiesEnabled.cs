using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RodskaNote.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class PropertiesEnabled: Attribute
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public int Order { get; set; }

        public PropertiesEnabled(string title, string desc, string cat, int order)
        {
            Title = title;
            Description = desc;
            Category = cat;
            Order = order;
        }


    }
}
