using System;
using RestSharp;
using System.Text.Json;
using System.Threading.Tasks;

namespace TG_Sender.Data
{
    class ReadValuesBTT
    {
        internal async Task<BTT_Model> SendHttpRequest()
        {
            SendMessage sendMessage = new();

            string pathUrl = $"http://127.0.0.1:{GetValues().Item3}/api/status";
            IRestResponse response = null;

            RestClient restClient = new(pathUrl);
            RestRequest restRequest = new();

            await Task.Factory.StartNew(() => {
                response = restClient.Get(restRequest);
            });

            if (response.ErrorException == null)
            {
                sendMessage.Errors = "";
                return ToModelFromJson(response);
            }
            else
            {
                sendMessage.Errors += $"Ошибка: {response.ErrorException.Message} \r\n" + "Проверьте порт!";
                return new BTT_Model { };
            }
        }

        private BTT_Model ToModelFromJson(IRestResponse responseMess)
        {
            return JsonSerializer.Deserialize<BTT_Model>(responseMess.Content);
        }

        private Tuple<string, string, string, string, string> GetValues()
        {
            WrtDataValue wrtData = new();
            return wrtData.ReadValue().Result;
        }
    }
}
