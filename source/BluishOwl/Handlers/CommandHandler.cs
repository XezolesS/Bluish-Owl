using BluishOwl.Datas;
using BluishOwl.Logging;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BluishOwl.Handlers
{
    internal static class CommandHandler
    {
        private static DiscordSocketClient Client = Initializer.Client;
        private static CommandService Command = Initializer.Command;
        private static IServiceProvider Service = Initializer.Service;

        internal static async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            // return if message is null
            if (message == null) 
                return;

            // return if message is sended by bot itself
            if (message.Author.Id == Client.CurrentUser.Id) 
                return;

            // return if message string dosen't contain any characters
            if (string.IsNullOrWhiteSpace(message.Content)) 
                return;

            // Get prefix
            GuildDataIO io = new GuildDataIO((message.Channel as SocketGuildChannel).Guild.Id);
            var prefix = io.Read().CommandPrefix;
            int argPos = 0;

            // Consider to be command if the message has prefix or mention to bot in front of it
            if (message.HasStringPrefix(prefix, ref argPos) ||
                message.HasMentionPrefix(Client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(Client, message);
                var result = await Command.ExecuteAsync(context, argPos, Service);

                if (result.IsSuccess)
                {
                    Logger.Info("Command", $"({context.Guild.Id}) {context.User} calls command \"{message}\"!");
                }
                else
                {
                    await message.Channel.SendMessageAsync(null, false, new EmbedBuilder()
                    {
                        Description = string.Format(result.ErrorReason),
                        Color = Color.Red
                    }.Build());

                    Logger.Error("Command", $"({context.Guild.Id}) {result.ErrorReason}");
                }
            }
        }
    }
}
