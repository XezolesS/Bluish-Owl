using BluishOwl.Datas;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BluishOwl.Modules
{
    [Group("prefix")]
    [Summary("Managing guild's command prefix.")]
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        [Summary("Display current command prefix of the guild.")]
        public async Task CommandPrefix()
        {
            GuildDataIO io = new GuildDataIO(Context.Guild.Id);
            GuildData guildData = io.Read();

            await ReplyAsync(string.Empty, false, new EmbedBuilder()
            {
                Description = string.Format("Current prefix is `{0}`", guildData.CommandPrefix),
                Color = Color.Green
            }.Build());
        }

        [Command]
        [Summary("Set command prefix for the guild.")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Priority(1)]
        public async Task CommandPrefix(string prefix)
        {
            GuildDataIO io = new GuildDataIO(Context.Guild.Id);
            GuildData guildData = io.Read();
            guildData.CommandPrefix = prefix;
            
            io.Write(guildData);

            await ReplyAsync(string.Empty, false, new EmbedBuilder()
            {
                Title = "Prefix Changed!",
                Description = string.Format("Command prefix of the guild has changed to `{0}`.", prefix),
                Color = Color.Green
            }.Build());
        }

        [Command("reset")]
        [Summary("Reset to default prefix(!)")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        [Priority(2)]
        public async Task CommandPrefixReset()
        {
            GuildDataIO io = new GuildDataIO(Context.Guild.Id);
            io.Write(new GuildData());

            await ReplyAsync(string.Empty, false, new EmbedBuilder()
            {
                Title = "Prefix Reset!",
                Description = string.Format("Command prefix of the guild has reset. (`!`)"),
                Color = Color.Green
            }.Build());
        }
    }
}
