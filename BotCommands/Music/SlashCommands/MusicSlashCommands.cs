using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus;
using DSharpPlus.Lavalink;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using DSharpPlus.CommandsNext.Converters;
using System.Reflection;
using DSharpPlus.EventArgs;

namespace darkwebkiller.BotCommands.Music.SlashCommands
{

    [SlashCommandGroup("music", "Perform music operations")]
    internal class MusicSlashCommands : ApplicationCommandModule
    {

        static List<LavalinkTrack> trackQueue = new List<LavalinkTrack>();
        static int videoIndex = 0;
        static bool playVideo = true;
        static int listNumber = 0;

        [SlashCommand("play", "Play the music")]
        public async Task PlayMusic(InteractionContext ctx, [Option("query", "Link or search query")] string query)
        {


            await ctx.DeferAsync();

            var userVC = ctx.Member?.VoiceState?.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member?.VoiceState == null || userVC == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter voice channel!!!"));
                return;

            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Connection is not Established!!!"));
                return;

            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter a valid voice channel!!!"));
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            await node.ConnectAsync(userVC);

            //if bot not connect the channel
            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (conn == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink failed to connect!!!"));
                return;
            }
            //find music
            var searchQuery = await node.Rest.GetTracksAsync(query.Trim());
            if (searchQuery.LoadResultType == LavalinkLoadResultType.NoMatches || searchQuery.LoadResultType == LavalinkLoadResultType.LoadFailed)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Failed to find music {ctx.Member.Mention} pls enter Plain,Youtube or SoundCloud link"));
                return;
            }

            var musicTrack = searchQuery.Tracks.FirstOrDefault();
            //add the track on music list
            trackQueue.Add(musicTrack);
            //get youtube url
            string youtubeUrl = musicTrack.Uri.ToString();
            //get youtube video id
            string videoId = youtubeUrl.Split("v=")[1];
            //get video thumbnail
            var thumbnailUrl = $"https://img.youtube.com/vi/{videoId}/hqdefault.jpg";
            listNumber += 1;
            

            //if track is null play the music
            if (conn.CurrentState.CurrentTrack == null && videoIndex <= trackQueue.Count - 1)
            {
                //play first video
                await PlayNextTrack();


                //play next videos
                conn.PlaybackFinished += async (s, e) =>
                {


                    try
                    {
                        videoIndex++;

                        if (videoIndex >= trackQueue.Count)
                        {
                            
                            playVideo = false;
                            return;
                        
                        }

                        if (playVideo)
                            await PlayNextTrack();

                    }catch (Exception err) 
                    {
                        playVideo = false;

                    
                    }


                };

            }
            var addListMessage = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Purple,
                Title = $"**#{listNumber} Video Added to list**",
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
                {
                    Url = thumbnailUrl
                },


            };

            addListMessage.AddField("🎵 Video Title", $"{musicTrack.Title}", true)
           .AddField("👤 Author", $"{musicTrack.Author}", true)
           .AddField("🔗 URL", $"{musicTrack.Uri}", false);


            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(addListMessage));


            async Task PlayNextTrack()
            {
                LavalinkTrack nextTrack;

                if (trackQueue.Count > 0)
                {

                    playVideo = true;
                    nextTrack = trackQueue[videoIndex];
                    await conn.PlayAsync(nextTrack);
                    
                    
                }

            }

        

        }

        [SlashCommand("skip","Skip the music")]
        public async Task SkipMusic(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pleasee enter voice channel!!!"));
                return;

            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Connection is not Established!!!"));
                return;

            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter a valid voice channel!!!"));
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();


            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (conn == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink failed to connect!!!"));
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No tracks are playing!!!"));
                return;
            }

            await conn.StopAsync();

            var skipEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Blue,
                Title = ":track_next: Track Skipped"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(skipEmbed));
        }

        [SlashCommand("pause","Pause the music")]
        public async Task PauseMusic(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pleasee enter voice channel!!!"));
                return;

            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Connection is not Established!!!"));
                return;

            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter a valid voice channel!!!"));
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            

            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (conn == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink failed to connect!!!"));
                return;
            }

            if(conn.CurrentState.CurrentTrack == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No tracks are playing!!!"));
                return;
            }

            await conn.PauseAsync();

            var pausedEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Blue,
                Title = ":arrow_forward: Track Paused"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(pausedEmbed));
        }

        [SlashCommand("resume","Resume the music")]
        public async Task ResumeMusic(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pleasee enter voice channel!!!"));
                return;

            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Connection is not Established!!!"));
                return;

            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter a valid voice channel!!!"));
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            

            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (conn == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink failed to connect!!!"));
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No tracks are playing!!!"));
                return;
            }

            await conn.ResumeAsync();

            var resumeEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Blue,
                Title = ":pause_button: Track Resumed"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(resumeEmbed));
        }

        [SlashCommand("stop","Stop the music")]
        public async Task StopMusic(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pleasee enter voice channel!!!"));
                return;

            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Connection is not Established!!!"));
                return;

            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter a valid voice channel!!!"));
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            

            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (conn == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink failed to connect!!!"));
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No tracks are playing!!!"));
                return;
            }

            await conn.StopAsync();

            var stopEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Red,
                Title = ":no_entry: Track Stopped"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(stopEmbed));

        }

        [SlashCommand("seek","Seek on the music")]
        public async Task SeekMusic(InteractionContext ctx, [Option("duration","skip duration (second) on music")] long duration)
        {
            await ctx.DeferAsync();

            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Pleasee enter voice channel!!!"));
                return;

            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Connection is not Established!!!"));
                return;

            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter a valid voice channel!!!"));
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();


            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (conn == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink failed to connect!!!"));
                return;
            }

            if (conn.CurrentState.CurrentTrack == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No tracks are playing!!!"));
                return;
            }

            TimeSpan seekPosition = TimeSpan.FromSeconds(duration);
            await conn.SeekAsync(seekPosition);

            var seekEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Blue,
                Title = ":fast_forward:  Track Seeked"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(seekEmbed));
        }

        [SlashCommand("leave","Leave voice channel")]
        public async Task LeaveChannel(InteractionContext ctx)
        {
            await ctx.DeferAsync();

            var userVC = ctx.Member.VoiceState.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();
            trackQueue.Clear();
            videoIndex = 0;
            listNumber = 0;

            if (ctx.Member.VoiceState == null || userVC == null)
            {
                await ctx.Channel.SendMessageAsync("Please enter a VC!!!");
                return;
            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.Channel.SendMessageAsync("Connection is not Established!!!");
                return;
            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.Channel.SendMessageAsync("Please enter a valid VC!!!");
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            

            var conn = node.GetGuildConnection(ctx.Member.VoiceState.Guild);
            if (conn == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Lavalink failed to connect!!!"));
                return;
            }


            await conn.DisconnectAsync();

            var nowLeavingEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Blue,
                Title = "I left from the discord channel"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(nowLeavingEmbed));

        }

        [SlashCommand("join","Join voice channel")]
        public async Task JoinMusic(InteractionContext ctx, [Option("channel","Select a voice channel")] DiscordChannel channel)
        {
            await ctx.DeferAsync();

            var userVC = ctx.Member?.VoiceState?.Channel;
            var lavalinkInstance = ctx.Client.GetLavalink();

            
            if (ctx.Member?.VoiceState == null || userVC == null)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter voice channel!!!"));
                return;

            }

            if (!lavalinkInstance.ConnectedNodes.Any())
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Connection is not Established!!!"));
                return;

            }

            if (userVC.Type != ChannelType.Voice)
            {
                await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Please enter a valid voice channel!!!"));
                return;
            }

            var node = lavalinkInstance.ConnectedNodes.Values.First();
            await node.ConnectAsync(channel);

            var discordJoinEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Blue,
                Title = "I join the discord channel"
            };

            await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(discordJoinEmbed));

        }


    
    }

}