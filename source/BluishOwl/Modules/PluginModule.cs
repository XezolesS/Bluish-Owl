using BluishOwl.Datas;
using BluishOwl.Generals;
using BluishOwl.Plugin;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BluishOwl.Modules
{
    [Group("plugin")]
    [Alias("pl")]
    [Summary("Check out what kind of plugins have been installed into this bot.")]
    public class PluginModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        [Alias("help")]
        public async Task CommandPlugin()
        {
            var PluginModuleCommands = ModuleHelpMessage.GetCommands("plugin");

            await ReplyAsync(string.Empty, false, new EmbedBuilder
            {
                Title = "Commands of plugin",
                Description = string.Join("\n", PluginModuleCommands),
                Color = Color.Blue
            }.Build());
        }

        [Command("list")]
        [Summary("Show the list of plugins which have installed.")]
        public async Task CommandPluginList()
        {
            string content = string.Empty;
            foreach (var plugin in PluginLoader.LoadedPlugins)
            {
                content += plugin.Name + "\n";
            }

            await ReplyAsync(string.Empty, false, new EmbedBuilder()
            {
                Title = "List of Plugins",
                Description = content,
                Color = Color.DarkGreen
            }.Build());
        }

        [Command("info")]
        [Summary("Shows details about specific plugin.")]
        public async Task CommandPluginInfo([Remainder]string pluginName)
        {
            foreach (var plugin in PluginLoader.LoadedPlugins)
            {
                if (pluginName.ToLower() == plugin.Name.ToLower())
                {
                    await ReplyAsync(string.Empty, false, new EmbedBuilder()
                    {
                        Title = plugin.Name,
                        Description = $"{plugin.Description}" + "\n\n" +
                            $"**Author** `{plugin.Author}`" + "\n" +
                            $"**Version** `{plugin.Version}`",
                        Color = Color.DarkGreen
                    }.Build());

                    return;
                }
            }

            await ReplyAsync(string.Empty, false, new EmbedBuilder()
            {
                Description = "**Usage**: plugin info `<Name of Plugin>`",
                Footer = new EmbedFooterBuilder()
                {
                    Text = $"The plugin that has name of {pluginName} is not found."
                },
                Color = Color.Red
            }.Build());
        }
    }
}
