using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace darkwebkiller.BotCommands.ChatConrol.SlashCommands
{
    internal class ChatControlSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("clean","clean messages")]
        public async Task CleanMessagesAsync(InteractionContext ctx, [Option("amount","message count")] long amount)
        {
            await ctx.DeferAsync();

            try
            {
                var messages = await ctx.Channel.GetMessagesAsync((int) amount + 1);

                await ctx.Channel.DeleteMessagesAsync(messages);


                var feedbackMessage = await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{amount} deleted messages"));
                await Task.Delay(3000);
                await feedbackMessage.DeleteAsync();
            }
            catch (Exception)
            {

                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"amount must be number"));
            }

         

        }


    }
}
