using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace darkwebkiller.BotCommands.Crypto.SlashCommands
{
    [SlashCommandGroup("crypto", "Perform crypto operations")]
    public class CryptoCoinSlashCommands : ApplicationCommandModule
    {

        [SlashCommand("price", "Get the coin price")]
        public async Task GetCryptoPrice(InteractionContext ctx, [Option("unit", "Unit of coin")] string unit)
        {

            await ctx.DeferAsync();

                
            
        }
    }
}
