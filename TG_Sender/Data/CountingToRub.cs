using RestSharp;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using TG_Sender.Models;

namespace TG_Sender.Data
{
    class CountingToRub
    {
        internal Tuple<decimal, float> GetQoute(decimal amount)
        {
            IRestResponse result = SendRequest(amount);

            USDquoteModel model = ToModel(result);

            return Tuple.Create(Convert.ToDecimal(model.data.sum_result), model.data.rate1);
        }

        private IRestResponse SendRequest(decimal amount)
        {
            string amountC = amount.ToString().Replace(',', '.');
            RestClient restClient = new("https://cash.rbc.ru/cash/json/converter_currency_rate/"+
                $"?currency_from=USD&currency_to=RUR&source=cbrf&sum={amountC}");
            RestRequest restRequest = new(Method.GET);

            IRestResponse response = restClient.Execute(restRequest);
            return response;
        }

        private USDquoteModel ToModel(IRestResponse responseJson)
        {
            return JsonSerializer.Deserialize<USDquoteModel>(responseJson.Content);
        }
    }
}
