using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace darkwebkiller.BotEvents.memberEvents
{
    public class MemberEnterTheServer
    {
        public async Task OnGuildMemberAdded(DiscordClient sender,GuildMemberAddEventArgs e) 
        {

            ulong channelId = 1284511080093192272; 
            var channel = await sender.GetChannelAsync(channelId);

            var welcomeEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.None,
                ImageUrl = e.Member.AvatarUrl,
                Title = $"{e.Member.DisplayName}",
                Description = $"Welcome to {e.Guild.Name} :wave:",
                

            };

            await channel.SendMessageAsync(welcomeEmbed);
        }


    }
}
