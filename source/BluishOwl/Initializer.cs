using BluishOwl.Datas;
using BluishOwl.Handlers;
using BluishOwl.Logging;
using BluishOwl.Plugin;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BluishOwl
{
    internal class Initializer
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static CommandService Command { get; set; }
        internal static IServiceProvider Service { get; set; }

        private BotData BotData;

        internal Initializer(BotData botConfig)
        {
            this.BotData = botConfig;
            InitializeBotAsync().GetAwaiter().GetResult();
        }

        private async Task InitializeBotAsync()
        {
            Client = new DiscordSocketClient();
            Command = new CommandService();
            Service = new ServiceCollection().AddSingleton(Client).AddSingleton(Command).BuildServiceProvider();

            foreach (var plugin in PluginLoader.LoadedPlugins)
            {
                try
                {
                    plugin.OnLoaded();
                }
                catch (Exception ex)
                {
                    Logger.Warning("Plugin", $"({plugin.Name}) {ex.Message}");
                }
            }

            // Logging
            Client.Log += Log;
            Command.Log += Log;

            // Authorize Guild Events
            Client.GuildAvailable += GuildAvailable;
            Client.JoinedGuild += JoinedGuild;

            // Command Handling
            Client.MessageReceived += (args) => CommandHandler.HandleCommandAsync(args);

            // Add Commands
            await Command.AddModulesAsync(Assembly.GetEntryAssembly(), Service);

            // Add commands from plugins
            foreach (var plugin in PluginLoader.LoadedPlugins)
            {
                await Command.AddModulesAsync(plugin.Assembly, Service);
            }

            // Login
            try
            {
                await Client.LoginAsync(TokenType.Bot, BotData.Token);
            }
            catch (Exception ex)
            {
                // If login failed, show exception message to console.
                Logger.Critical("Login", ex.Message);
            }

            // Start
            await Client.StartAsync();

            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
            Logger.Log(arg.Severity, arg.Source, arg.Message);
            return Task.CompletedTask;
        }

        private Task JoinedGuild(SocketGuild arg)
        {
            Logger.Info("Guild", "Bot joined guild \"{0}({1})\".", arg.Name, arg.Id);

            GuildDataIO guildDataIO = new GuildDataIO(arg.Id);
            if (!guildDataIO.DirectoryExists())
                guildDataIO.CreateDirectory();

            if (!guildDataIO.Exists())
                guildDataIO.Write(new GuildData());

            return Task.CompletedTask;
        }

        private Task GuildAvailable(SocketGuild arg)
        {
            Logger.Info("Guild", "Found joined guild \"{0}({1})\".", arg.Name, arg.Id);

            GuildDataIO guildDataIO = new GuildDataIO(arg.Id);
            if (!guildDataIO.DirectoryExists())
                guildDataIO.CreateDirectory();

            if (!guildDataIO.Exists())
                guildDataIO.Write(new GuildData());

            return Task.CompletedTask;
        }
    }
}
