using darkwebkiller.config.JsonReader;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace darkwebkiller.BotCommands.ChatGpt.SlashCommands
{

    [SlashCommandGroup("chatgpt", "Perform chatgpt operations")]
    internal class ChatGptSlashCommands : ApplicationCommandModule
    {
      
        [SlashCommand("text", "Select a ChatGpt model")]
        public async Task TextModel(InteractionContext ctx, [Option("promt","write a promt")] string prompt)
        {

            await ctx.DeferAsync();

            var jsonReader = new JSONReader();
            await jsonReader.ReadJSON();

            string apiKey = jsonReader.ChatGptApi;
            string apiUrl = "https://api.openai.com/v1/chat/completions";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new {role = "user", content = prompt}
                    },

                    //max_tokens = 150 / if you want text limit
                };

                var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, content);
                var responseString = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);



            
                var ChatGptAnswer = new DiscordEmbedBuilder()
                {
                    Color = ctx.Member.Color,
                    Title = "**ChatGpt Chat**",

                };

                ChatGptAnswer
                .AddField("👤 User", $"{prompt}", false)
                .AddField("🤖 Assistant", $"{responseObject?.choices[0].message.content ?? "Error: Enable response to ChatGpt"}", false);


                await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(ChatGptAnswer));

                //await ctx.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{responseObject?.choices[0].message.content ?? "Error: Enable response to ChatGpt"}"));



            }

        }

        //[SlashCommand("image-generate", "input a promt for image generation")]
        //public async Task ImageModel(InteractionContext ctx, [Option("promt","write a promt")] string userPrompt)
        //{
        //    await ctx.DeferAsync();

        //    var jsonReader = new JSONReader();
        //    await jsonReader.ReadJSON();

        //    string apiKey = jsonReader.ChatGptApi;
        //    string apiUrl = "https://api.openai.com/v1/images/generations";

        //    using(HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        //        var requestBody = new
        //        {
        //            model = "dall-e-3",
        //            prompt = userPrompt,
        //            n = 1,
        //            size = "1024x1024"

        //        };

                
        //        var jsonRequestBody = JsonConvert.SerializeObject(requestBody);
        //        var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync(apiUrl, content);
        //        var responseString = await response.Content.ReadAsStringAsync();
        //        var responseJson = System.Text.Json.JsonDocument.Parse(responseString);
        //        var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);


        //        var ChatGptAnswer = new DiscordEmbedBuilder()
        //        {
        //            Color = ctx.Member.Color,
        //            Title = "**ChatGpt Image Generation**",
        //            ImageUrl = responseObject?.data[0].url ?? "Error: Image dindnt generete",

        //        };

        //        ChatGptAnswer
        //        .AddField("👤 User", $"{userPrompt}", false);



        //        await ctx.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(ChatGptAnswer));




        //    }


        //}

    }

}
