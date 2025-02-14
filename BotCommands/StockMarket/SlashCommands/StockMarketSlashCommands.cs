using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace darkwebkiller.BotCommands.StockMarket.SlashCommands
{
    [SlashCommandGroup("stock", "Perform stock operations")]
    internal class StockMarketSlashCommands
    {
        [SlashCommand("market","Ckeck the stock market value")]
        public async Task StockValue(InteractionContext ctx, [Option("unit","unit of stock")] string unit)
        {




        }

    }
}
