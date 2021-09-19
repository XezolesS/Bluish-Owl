using BluishOwl.Datas;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluishOwl.Modules
{
    [Group("help")]
    [Summary("Shows informations about commands.")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        IEnumerable<CommandInfo> Commands = Initializer.Command.Commands;

        [Command(RunMode = RunMode.Async)]
        [Summary("Shows all commands.")]
        public async Task CommandHelp()
        {
            var io = new GuildDataIO(Context.Guild.Id);
            var prefix = io.Read().CommandPrefix;
    
            List<string> commandsList = new List<string>();
            foreach (var cmd in Commands)
            {
                commandsList.Add(cmd.Aliases[0]);
            }

            commandsList = commandsList.Distinct().ToList();

            string commandsText = string.Empty;
            foreach (var cmd in commandsList)
            {
                commandsText += "`-` " + prefix + cmd + "\n";
            }

            await ReplyAsync(string.Empty, false, new EmbedBuilder
            {
                Title = string.Format("Total {0} {1}", 
                    commandsList.Count, 
                    commandsList.Count == 1 ? "command has been found!" : "commands have been found!"),
                Description = commandsText,
                Footer = new EmbedFooterBuilder
                {
                    Text = $"{prefix}help <pluginName> for more detail."
                },
                Color = Color.Green
            }.Build());
        }

        [Command(RunMode = RunMode.Async)]
        [Summary("Shows details about specific command.")]
        public async Task CommandHelp([Remainder]string command)
        {
            var io = new GuildDataIO(Context.Guild.Id);
            var prefix = io.Read().CommandPrefix;

            command = command.Replace(prefix, "");

            List<EmbedFieldBuilder> CommandInfoFieldList = new List<EmbedFieldBuilder>();
            var foo = Commands.Where(cmd => string.Equals(command, cmd.Aliases[0]));
            foreach (var cmd in foo)
            {
                string parameterText = null;
                foreach (var param in cmd.Parameters)
                {
                    parameterText += $"`<{param}>` ";
                }

                string aliasesText = null;
                foreach (var alias in cmd.Aliases)
                {
                    if (alias != cmd.Aliases[0])
                        aliasesText += $"`{alias}` ";
                }

                /*
                string permissionText = null;
                foreach (var attribute in cmd.Preconditions)
                {
                    Logging.Logger.Debug("Help", attribute.ToString());

                    if (attribute is RequireUserPermissionAttribute)
                    {
                        permissionText = (attribute as RequireUserPermissionAttribute).GuildPermission.ToString();
                    }
                }*/

                CommandInfoFieldList.Add(new EmbedFieldBuilder
                {
                    Name = $"**{prefix}{command}** {parameterText}",
                    Value = $"{cmd.Summary ?? "No description."}\n" +
                        aliasesText != null ? "" : $"**Aliases**: {aliasesText}\n", // if aliasesText is null, append nothing. else append aliases
                        //$"**Permissions**: `{permissionText ?? "Any Users"}`",
                    IsInline = false
                });                           
            }

            if (CommandInfoFieldList.Count == 0)
            {
                await ReplyAsync(string.Empty, false, new EmbedBuilder
                {
                    Description = $"Command `{prefix}{command}` has not found.",
                    Color = Color.Red
                }.Build());
            }
            else
            {
                await ReplyAsync(string.Empty, false, new EmbedBuilder
                {
                    Title = $"All about `{prefix}{command}`",
                    Fields = CommandInfoFieldList,
                    Color = Color.Blue
                }.Build());
            }
        }
    }
}
