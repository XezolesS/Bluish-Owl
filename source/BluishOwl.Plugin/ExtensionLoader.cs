using BluishOwl.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;

namespace BluishOwl.Plugin
{
    public static class ExtensionLoader
    {
        private static readonly string ExtensionDirectory = Directory.GetCurrentDirectory() + @"\" + "Extensions";
        private static readonly string ExtensionWindowsDirectory = ExtensionDirectory + @"\" + "win";
        private static readonly string ExtensionUnixDirectory = ExtensionDirectory + @"\" + "unix";

        public static void Load()
        {
            CreateDirectoriesIfNotExist();

            // Load Extension dlls.
            foreach (var assembly in GetExtensionAssemblies())
            {
                Assembly.LoadFile(assembly.Location);
                Logger.Info("Plugin", $"Extension {assembly.FullName.Split(',')[0]}({assembly.ImageRuntimeVersion}) has successfully loaded.");
            }
        }

        private static IEnumerable<Assembly> GetExtensionAssemblies()
        {
            var dlls = Directory.GetFiles(ExtensionDirectory, "*.dll", SearchOption.TopDirectoryOnly)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToList();

            // If runtime OS is Windows, add assemblies located in \Extensions\win
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var winDlls = Directory.GetFiles(ExtensionWindowsDirectory, "*.dll", SearchOption.AllDirectories)
                    .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                    .ToArray();

                dlls.AddRange(winDlls);
            }

            // If runtime OS is Unix, add assemblies located in \Extensions\unix
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var unixDlls = Directory.GetFiles(ExtensionUnixDirectory, "*.dll", SearchOption.AllDirectories)
                    .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                    .ToArray();

                dlls.AddRange(unixDlls);
            }

            return dlls;
        }

        private static void CreateDirectoriesIfNotExist()
        {
            if (!Directory.Exists(ExtensionDirectory))
                Directory.CreateDirectory(ExtensionDirectory);

            if (!Directory.Exists(ExtensionWindowsDirectory))
                Directory.CreateDirectory(ExtensionWindowsDirectory);

            if (!Directory.Exists(ExtensionUnixDirectory))
                Directory.CreateDirectory(ExtensionUnixDirectory);
        }
    }
}
