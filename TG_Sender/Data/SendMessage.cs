using RestSharp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TG_Sender.Data
{
    class SendMessage
    {
        internal string Errors { get; set; }
        private int Update_Id = 0;

        internal void SendData()
        {
            Program.ThreadNow = new Thread(async () => {

                while (true)
                {
                    int tm = Convert.ToInt32(GetValues().Item5) * 1000;

                    await SendStatus();
                    ShemeReturn(Convert.ToInt32(GetValues().Item5));

                    Thread.Sleep(tm);
                }           
            });

            Program.ThreadNow.IsBackground = true;
            Program.ThreadNow.Start();    
        }

        private async Task<IRestResponse> SendStatus()
        {
            ReadValuesBTT bTT = new();
            MarketCapAPI marketCap = new();
            CountingToRub toRub = new();
            BTT_Model model = await bTT.SendHttpRequest();

            string api_key = GetValues().Item1;
            string name_server = GetValues().Item2;
            string chat_id = GetValues().Item4;
            Update_Id += 1;

            Tuple<string, float, DateTime> resMarketCap = marketCap.GetMarketCap();

            decimal balance = Convert.ToDecimal(model.balance) / Convert.ToDecimal(1000000);
            decimal profitUSD = balance * Convert.ToDecimal(resMarketCap.Item2);

            Tuple<decimal, float> amountRub = toRub.GetQoute(profitUSD);

            string pathUrl = $"https://api.telegram.org/bot{api_key}/sendMessage";
            string message = $"N: {Update_Id} Server: {name_server}; _ BTT: {balance}; _ Now peers: {model.peers} \r\n"+
                    $"Profit: 💵{ + profitUSD}; 💱 ₽{amountRub.Item1} \r\n" + 
                    "_________________________________________" +
                    $"\r\n Quotes {resMarketCap.Item1}: 💲{resMarketCap.Item2} USD; Ruble: 💲{amountRub.Item2} USD" +
                    $"\r\n Последнее обновление: {resMarketCap.Item3}";

            IRestResponse response = null;   

            try
            {  
                RestClient restClient = new(pathUrl);
                RestRequest restRequest = new(Method.GET);

                restRequest.AddParameter("chat_id", chat_id).AddParameter("text", message);

                response = await restClient.ExecuteAsync(restRequest);

                return response;
            }
            catch (Exception ex)
            {
                Errors += "\r\n" + ex.Message;
                return response;
            }
        }

        private Tuple<string, string, string, string, string> GetValues()
        {
            WrtDataValue wrtData = new();
            return wrtData.ReadValue().Result;
        }

        private void ShemeReturn(int timer)
        {
            double tm = Convert.ToDouble(timer) / Convert.ToDouble(60);
            Console.WriteLine("\r\n XXX \r\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Сообщение отправленно... Num: {Update_Id}\r\n" +
                $"Следующая отправка через: {tm} минут!\r\n");
            Console.ResetColor();
            Console.WriteLine("КОМАНДА: \r\n");
        }
    }
}
