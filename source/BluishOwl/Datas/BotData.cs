using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BluishOwl.Datas
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class BotData
    {
        [JsonProperty]
        internal long ClientId { get; private set; }

        [JsonProperty]
        internal string Token { get; private set; }

        [JsonProperty]
        internal int Permissions { get; private set; }

        [JsonConstructor]
        internal BotData(long clientId = 0, string token = "Token Here", int permissions = 0)
        {
            this.ClientId = clientId;
            this.Token = token;
            this.Permissions = permissions;
        }
    }

    internal static class BotDataIO 
    {
        readonly static string ConfigFileName = "bot.config";

        internal static void Write()
        {
            using (StreamWriter sw = new StreamWriter(ConfigFileName))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                BotData botData = new BotData();
                string serialized = JsonConvert.SerializeObject(botData, Formatting.Indented);
                sw.Write(serialized);
            }
        }

        internal static BotData Read()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader(ConfigFileName))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<BotData>(reader);
            }
        }

        internal static bool Exists()
        {
            return File.Exists(ConfigFileName);
        }
    }
}
