using BluishOwl.Logging;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;

namespace BluishOwl.Plugin
{
    public static class PluginLoader
    {
        private static readonly string PluginDirectory = Directory.GetCurrentDirectory() + @"\" + "Plugins";
        private static readonly string PluginDisabledDirectory = PluginDirectory + @"\" + "Disabled";

        public static List<Plugin> LoadedPlugins { get; private set; } = new List<Plugin>();

        public static void Load()
        {
            CreateDirectoriesIfNotExist();

            // Load Plugn dlls.
            foreach (var assembly in GetPluginAssemblies())
            {
                var configuration = new ContainerConfiguration().WithAssembly(assembly);
                using (var container = configuration.CreateContainer())
                {
                    if (container.TryGetExport(out Plugin plugin))
                    {                   
                        // Set plugin's information.
                        plugin.PluginInfo = GetPluginAttribute(plugin);
                        plugin.DescriptionInfo = GetDescriptionAttribute(plugin);
                        plugin.Assembly = assembly;

                        LoadedPlugins.Add(plugin); // Add plugin to Plugins list
                        Logger.Info("Plugin", $"Plugin {plugin.Name}({plugin.Version}) by {plugin.Author} has successfully loaded.");
                    }                   
                }
            }
        }

        private static IEnumerable<Assembly> GetPluginAssemblies()
        {
            var dlls = Directory.GetFiles(PluginDirectory, "*.dll", SearchOption.AllDirectories)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToList();

            for (int i = dlls.Count - 1; i >= 0; i--)            
                if (dlls[i].Location.StartsWith(PluginDisabledDirectory))
                    dlls.RemoveAt(i);
            
            return dlls;
        }

        private static PluginAttribute GetPluginAttribute(Plugin plugin)
        {
            try
            {
                var attributes = Attribute.GetCustomAttributes(plugin.GetType());
                foreach (var attr in attributes)
                {
                    if (attr is PluginAttribute)
                    {
                        return (PluginAttribute)attr;
                    }
                }

                return null;
            }
            catch (SystemException)
            {
                return null;
            }
        }

        private static DescriptionAttribute GetDescriptionAttribute(Plugin plugin)
        {
            try
            {
                var attributes = Attribute.GetCustomAttributes(plugin.GetType());
                foreach (var attr in attributes)
                {
                    if (attr is DescriptionAttribute)
                    {
                        return (DescriptionAttribute)attr;
                    }
                }

                return null;
            }
            catch (SystemException)
            {
                return null;
            }
        }

        private static void CreateDirectoriesIfNotExist()
        {
            if (!Directory.Exists(PluginDirectory))
                Directory.CreateDirectory(PluginDirectory);

            if (!Directory.Exists(PluginDisabledDirectory))
                Directory.CreateDirectory(PluginDisabledDirectory);
        }
    }
}
