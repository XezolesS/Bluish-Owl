using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RNGExtension
{
    public class Module : ModuleBase<SocketCommandContext>
    {
        [Command("random")]
        [Alias("rng")]
        public async Task CommandRNGRange(int min, int max)
        {
            Random rand = new Random();
            var number = rand.Next(min, max);

            await ReplyAsync(string.Empty, false, new EmbedBuilder
            {
                Description = "I generated: " + number,
                Color = Color.Blue
            }.Build());
        }
    }
}
