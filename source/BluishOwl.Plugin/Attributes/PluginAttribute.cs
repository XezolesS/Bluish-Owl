using System;
using System.Collections.Generic;
using System.Composition;
using System.Text;

namespace BluishOwl.Plugin
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginAttribute : ExportAttribute 
    {
        public string Name { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; } 

        public PluginAttribute(string name, string author, string version) : base(typeof(Plugin))
        {
            this.Name = name;
            this.Author = author;
            this.Version = version;
        }
    }
}
