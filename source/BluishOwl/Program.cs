using BluishOwl.Datas;
using BluishOwl.Logging;
using BluishOwl.Plugin;
using Discord;
using Newtonsoft.Json;
using System;
using System.IO;

namespace BluishOwl
{
    // branch testing

    internal class Program
    {
        static void Main(string[] args)
        {
            // If config file does not exists, create one and exit the program.
            if (!BotDataIO.Exists())
            {
                Logger.Info("Config", "\"bot.config\" has not been found! Trying to create new one...");

                BotDataIO.Write();

                Logger.Info("Config", "\"bot.config\" has been created. Please fill up all the informations and restart.");
                return;
            }

            // Load Plugins
            PluginLoader.Load();

            BotData botData = BotDataIO.Read();
            Logger.Info("Config", "\"bot.config\" has been found!\n\n" +
                $"\tClientId: {botData.ClientId}\n" +
                $"\tToken: {botData.Token}\n" +
                $"\tPermissions: {botData.Permissions}\n" +
                $"\tInvite URL: https://discordapp.com/oauth2/authorize?&client_id={botData.ClientId}&scope=bot&permissions={botData.Permissions}\n");

            new Initializer(botData);
        }
    }
}
