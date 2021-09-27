using RestSharp;
using System;
using System.Text.Json;
using TG_Sender.Models;

namespace TG_Sender.Data
{
    class MarketCapAPI
    {
        private const string ApiKey = "6f9138b6-422d-4ef7-a679-a401fa7225ae";
        private const string URL_Path = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/listings/latest";

        internal Tuple<string, float, DateTime> GetMarketCap()
        {
            string nameCoin = string.Empty;
            float capitalizationUSD = 0;
            DateTime refreshDate = DateTime.Now;

            ACMC_Model model = ToModel(GetData());

            for (int i = 0; i < model.data.Length; i++)
            {
                if (model.data[i].id == 3718)
                {
                    nameCoin = model.data[i].name;
                    capitalizationUSD = model.data[i].quote.USD.price;
                    refreshDate = model.data[i].quote.USD.last_updated;
                }
            }
            return Tuple.Create(nameCoin, capitalizationUSD, refreshDate);
        }    

        private IRestResponse GetData()
        {
            RestClient restClient = new(URL_Path);
            RestRequest restRequest = new(Method.GET);
            IRestResponse response = null;

            restRequest.AddHeader("X-CMC_PRO_API_KEY", ApiKey);
            response = restClient.Execute(restRequest);
            if (response.ErrorException == null)
            {
                return response;
            }
            else
            {
                SendMessage sm = new();
                string ex = response.ErrorException.Message;

                sm.Errors += ex;
                return response;
            }           
        }

        private ACMC_Model ToModel(IRestResponse data)
        {
            return JsonSerializer.Deserialize<ACMC_Model>(data.Content);
        }
    }
}
