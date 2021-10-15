using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

public class Modules : ModuleBase<SocketCommandContext>
{
	[Command("example")]
	public async Task CommandExample()
    {
        await ReplyAsync("*This is the Message from Example Plugin.*");
    }
}
