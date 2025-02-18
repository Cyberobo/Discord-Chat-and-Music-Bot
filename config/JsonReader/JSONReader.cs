﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace darkwebkiller.config.JsonReader
{
    internal class JSONReader
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public string ChatGptApi { get; set; }
        public string CryptoApi { get; set; }
        public string StockMarketApi { get; set; }
        public string CivitAiApi { get; set; }

        public async Task ReadJSON()
        {
            using (StreamReader sr = new StreamReader("config.json"))
            {
                string json = await sr.ReadToEndAsync();
                JSONStructure data = JsonConvert.DeserializeObject<JSONStructure>(json);

                token = data.token;
                prefix = data.prefix;
                ChatGptApi = data.ChatGptApi;
                CryptoApi = data.CryptoApi;
                StockMarketApi = data.StockMarketApi;
                CivitAiApi = data.CivitAiApi;

            }

        }
    }

    internal sealed class JSONStructure
    {
        public string token { get; set; }
        public string prefix { get; set; }
        public string ChatGptApi { get; set; }
        public string CryptoApi { get; set; }
        public string StockMarketApi { get; set; }
        public string CivitAiApi { get; set; }
    }


}
