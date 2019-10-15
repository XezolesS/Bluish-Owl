using BluishOwl.Datas;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BluishOwl.Generals
{
    public static class ModuleHelpMessage
    {
        public static IEnumerable<string> GetCommands(string moduleName)
        {
            IEnumerable<CommandInfo> Commands = Initializer.Command.Commands;
            List<string> PluginModuleCommandList = new List<string>();

            foreach (var cmd in Commands)
            {
                if (cmd.Module.Group == moduleName)
                {
                    PluginModuleCommandList.Add(cmd.Aliases[0]);
                }
            }

            return PluginModuleCommandList;
        }
    }
}
