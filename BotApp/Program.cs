using darkwebkiller.BotCommands.Crypto;
using darkwebkiller.config.BotRun;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using System.Runtime.CompilerServices;

namespace darkwebkiller.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            
            RunDiscord bot = new RunDiscord();
            await bot.RunBot();

            
        }
    }
}

