using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using darkwebkiller.config.JsonReader;


namespace darkwebkiller.BotCommands.CivitAi.SlashCommands
{

    [SlashCommandGroup("civitai", "Perform civitai operations")]
    internal class CivitAiSlashCommands : ApplicationCommandModule
    {
        [SlashCommand("text-to-image", "text to image generate")]
        public async Task TextToImageModel(InteractionContext ctx, [Option("promt", "write a promt")] string prompt)
        {

            await ctx.DeferAsync();

            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            string apiKey = jsonReader.CivitAiApi;
            string apiUrl = "https://civitai.com/api/v1/images";

            


        }
    }
}
