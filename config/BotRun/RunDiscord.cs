using DSharpPlus.CommandsNext;
using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using darkwebkiller.config.JsonReader;
using darkwebkiller.BotCommands;
using System.Collections;
using darkwebkiller.BotCommands.Music.SlashCommands;
using DSharpPlus.SlashCommands;
using DSharpPlus.Net;
using DSharpPlus.Lavalink;
using darkwebkiller.BotCommands.Crypto.SlashCommands;
using darkwebkiller.BotEvents.memberEvents;
using darkwebkiller.BotCommands.ChatConrol.SlashCommands;
using darkwebkiller.BotCommands.ChatGpt.SlashCommands;
using darkwebkiller.BotCommands.CivitAi.SlashCommands;

namespace darkwebkiller.config.BotRun
{
    internal sealed class RunDiscord
    {
        private static DiscordClient client { get; set; }
        private static CommandsNextExtension commands { get; set; }

        public async Task RunBot()
        {
            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();


            //Client config
            var discordConfig = new DiscordConfiguration()
            {

                Intents = DiscordIntents.All,
                Token = jsonReader.token!,
                TokenType = TokenType.Bot,
                AutoReconnect = true,

            };

            client = new DiscordClient(discordConfig);
            client.Ready += ClientReady;

            //-----------------------------------------------------------

            //Prefix Commands config
            var commandConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false

            };

            commands = client.UseCommandsNext(commandConfig);


            //slash commands config
            var slashCommandConfig = client.UseSlashCommands();
            //Register command classes
            slashCommandConfig.RegisterCommands<MusicSlashCommands>();
            slashCommandConfig.RegisterCommands<ChatGptSlashCommands>();
            slashCommandConfig.RegisterCommands<ChatControlSlashCommands>();
            slashCommandConfig.RegisterCommands<CivitAiSlashCommands>();
            
         
            //-----------------------------------------------------------

            //lavalink configuration
            var endPoint = new ConnectionEndpoint()
            {
                Hostname = "lava-v3.ajieblogs.eu.org",
                Port = 443,
                Secured = true

            };

            var lavalinkConfig = new LavalinkConfiguration()
            {
                Password = "https://dsc.gg/ajidevserver",
                RestEndpoint = endPoint,
                SocketEndpoint = endPoint
            };

            var lavalink = client.UseLavalink();

            //-----------------------------------------------------------

            //Dc Member Events
            
                //Member welcome message
            MemberEnterTheServer memberEvent = new MemberEnterTheServer();
            client.GuildMemberAdded += memberEvent.OnGuildMemberAdded;



            //-----------------------------------------------------------

            await client.ConnectAsync();
            await lavalink.ConnectAsync(lavalinkConfig);
            await Task.Delay(-1);

        }

        private static Task ClientReady(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }



    }


}
