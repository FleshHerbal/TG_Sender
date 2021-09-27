using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TG_Sender.Data
{
    class WrtDataValue
    {
        private string NameServer { get; set; }
        private string API { get; set; }
        private string NameBot { get; set; }
        private string Port { get; set; }
        private bool IsPort { get; set; }

        private const int Size = 1024;

        internal WrtDataValue(string nameSrv, string api, string nameBot) 
        {
            NameServer = nameSrv;
            API = api;
            NameBot = nameBot;
        }
        internal WrtDataValue(string port, bool isPort = false)
        {
            Port = port;
            IsPort = isPort;
        }
        internal WrtDataValue() { }

        internal async Task<Tuple<string, string, string, string, string>> ReadValue()
        {
            StringBuilder stringBuilder = new(Size);
            string[] keys = new string[] { "Api_key", "Name_Server", "Port", "Chat_id", "Timeout" };
            string[] values = new string[5];
            string path = GetPathINI();

            await Task.Factory.StartNew(()=> {
                for (int i = 0; i < keys.Length; i++)
                {
                    GetFrom_INI("values", keys[i], null, stringBuilder, Size, path);
                    values[i] = stringBuilder.ToString();
                }
            });
            return Tuple.Create(values[0], values[1], values[2], values[3], values[4]);
        }

        internal void WriteTM(int tm)
        {
            new Thread(() => {
                WriteTO_INI("values", "Timeout", tm.ToString(), GetPathINI());
            }).Start();
        }

        internal void WriteValue()
        {
            if (IsPort == false)
            {
                string[] values = new string[] { API, NameServer, NameBot };
                string[] keys = new string[] { "Api_key", "Name_Server", "Chat_id" };
                string section = "values";
                string path = GetPathINI();

                Action action = new(() =>
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        WriteTO_INI(section, keys[i], values[i], path);
                    }
                });

                new Thread(() => { action(); }).Start();
            }
            else
            {
                new Thread(()=> {
                    WriteTO_INI("values", "Port", Port.ToString(), GetPathINI());
                }).Start();
            }
        }

        private string GetPathINI()
        {
            string path = Directory.GetCurrentDirectory() + "\\value.ini";
            return path;
        }

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetFrom_INI(string section, string key, string def, StringBuilder buffer,
            int size, string path);
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private static extern int WriteTO_INI(string section, string key, string value, string path);
    }
}
