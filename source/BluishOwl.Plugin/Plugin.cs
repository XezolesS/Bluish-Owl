using System;
using System.Reflection;

namespace BluishOwl.Plugin
{
    public abstract class Plugin
    {
        // Attribute Fields
        internal PluginAttribute PluginInfo { private get; set; }
        internal DescriptionAttribute DescriptionInfo { private get; set; }

        // Properties

        /// <summary>
        /// Name of the plugin.
        /// </summary>
        public string Name { get => PluginInfo.Name; }

        /// <summary>
        /// Author of the plugin.
        /// </summary>
        public string Author { get => PluginInfo.Author; }

        /// <summary>
        /// Version of the plugin.
        /// </summary>
        public string Version { get => PluginInfo.Version; }

        /// <summary>
        /// Description of the plugin.
        /// </summary>
        public string Description { get => DescriptionInfo.Text ?? "No description given."; }

        /// <summary>
        /// Assembly of the plugin.
        /// </summary>
        public Assembly Assembly { get; internal set; }


        // Abstract Methods

        /// <summary>
        /// Tasks run when plugin loaded.
        /// </summary>
        public abstract void OnLoaded();
    }
}
