using System;
using System.Threading;
using System.Threading.Tasks;
using TG_Sender.Data;

namespace TG_Sender
{
    class Program
    {
        private string ApiKey { get; set; }
        private string NameServer { get; set; }
        private string NameBot { get; set; }
        static internal Thread ThreadNow { get; set; }
        internal bool Stoped = true;

        static async Task Main()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Messege to Telegram");
            Console.WriteLine("___________________");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;

            Program program = new();

            Tuple<bool, string> res = await WriteCommands(">info");
            Console.WriteLine("\r\n" + res.Item2);

            Starting();

            Console.ReadKey();      
        }

        static private async Task Starting() 
        {
            Console.WriteLine("\r\n КOMAНДА:");
            string comands = Console.ReadLine();
            Program program = new();

            if (comands != "")
            {
                Tuple<bool, string> result_cmd = await WriteCommands(comands);

                if (result_cmd.Item1 == false)
                {
                    Console.WriteLine(result_cmd.Item2);
                    await Starting();
                }
                else
                    Console.WriteLine(result_cmd.Item2);
            }
        }

        static private async Task<Tuple<bool, string>> WriteCommands(string cmd)
        {
            Program Program = new();

            switch (cmd)
            {
                case ">info":
                    return Tuple.Create(false, "Информация: >info; \r\n" +
                        "Натсройки: >settings; \r\n" + "Старт программы: >start \r\n" +
                         "Остановка программы: >stop \r\n" + "Порт BTT: >port; \r\n"
                        + "Timeout: >tm; \r\n");

                case ">settings":
                    bool Is_setKey = Setting();

                    if (Is_setKey == true)
                    {
                        return Tuple.Create(false, "Настройки сохранены!");
                    }
                    else
                        return Tuple.Create(false, "Настройки не сохранены!");

                case ">port":
                    Console.WriteLine("Введите номер! порта из 'speed.btt.network':");
                    string port = Console.ReadLine();

                    if (port != "")
                    {
                        WrtDataValue wrtDataValue = new(port, true);
                        wrtDataValue.WriteValue();

                        return Tuple.Create(false, "Порт добавлен!");
                    }
                    else return Tuple.Create(false, "Порт не записан!");

                case ">tm":
                    Console.WriteLine("\r\n Установите таймаут отправки сообщений в сек.:");

                    try
                    {
                        int tm = Convert.ToInt32(Console.ReadLine());

                        WrtDataValue wrtDataValue = new();
                        wrtDataValue.WriteTM(tm);

                        return Tuple.Create(false, "\r\n Таймаут обновлен!");
                    }
                    catch (Exception)
                    {
                        return Tuple.Create(false, "\r\n Ты когда-нибудь видел: \" птиц - секунд\"???? ");
                    }

                case ">start":
                    SendMessage sendMessage = new();

                    sendMessage.SendData();

                    if (sendMessage.Errors == null)
                    {       
                        return Tuple.Create(false, "Сарт цикла отправки сообщений......");
                    }
                    else
                        return Tuple.Create(false, sendMessage.Errors);

                case ">stop":
                    Environment.Exit(0);
                    return Tuple.Create(true, "Отсановленно!");

                default:  return Tuple.Create(false, "Не поступило команды или введена неверная команда!");
            }
        }

        static private bool Setting()
        {
            Program program = new();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\r\n Следующие настройки необходимо ввести единоразово и без ошибок!");
            Console.ResetColor();

            Console.WriteLine("\r\n Ваедите API_token бота: (!!!)");
            program.ApiKey = Convert.ToString(Console.ReadLine());

            Console.WriteLine("\r\n Укажите ID чата: (!!!)");
            program.NameBot = Convert.ToString(Console.ReadLine());

            Console.WriteLine("\r\n Введите название вашего сервера: (!!!)");
            program.NameServer = Convert.ToString(Console.ReadLine());

            if (program.ApiKey.Length > 10 && program.NameServer.Length > 1)
            {
                WrtDataValue wrtDataValue = new(program.NameServer, program.ApiKey, program.NameBot);
                wrtDataValue.WriteValue();

                Console.WriteLine("\r\n Не забудте установить порт '>port'! \r\n ");

                return true;
            }
            else return false;
        }
    }
}
