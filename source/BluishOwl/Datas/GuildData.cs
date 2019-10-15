using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BluishOwl.Datas
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GuildData
    {
        [JsonProperty]
        public string CommandPrefix { get; set; }

        [JsonConstructor]
        public GuildData(string cmdPrefix = "!")
        {
            this.CommandPrefix = cmdPrefix;
        }
    }

    public class GuildDataIO
    {
        private string GuildDataDirectory;
        private readonly string GuildDataFile = "guild.json";

        public GuildDataIO(ulong guildId)
        {
            this.GuildDataDirectory = @"Datas/" + guildId;
        }

        public void CreateDirectory()
        {
            Directory.CreateDirectory(GuildDataDirectory);
        }

        public bool DirectoryExists()
        {
            return Directory.Exists(GuildDataDirectory);
        }

        public void Write(GuildData data)
        {
            using (StreamWriter sw = new StreamWriter(GuildDataDirectory + @"/" + GuildDataFile))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                string serialized = JsonConvert.SerializeObject(data, Formatting.Indented);
                sw.Write(serialized);
            }
        }

        public GuildData Read()
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader sr = new StreamReader(GuildDataDirectory + @"/" + GuildDataFile))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<GuildData>(reader);
            }
        }

        public bool Exists()
        {
            return File.Exists(GuildDataDirectory + @"/" + GuildDataFile);
        }

        /*
        public bool IsCorrectFormat()
        {
            try
            {
                Read();
            }
        }
        */
    }
}
