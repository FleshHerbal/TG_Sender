using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG_Sender.Models
{
    class USDquoteModel
    {
        public Meta meta { get; set; }
        public Data data { get; set; }

        public class Meta
        {
            public float sum_deal { get; set; }
            public string source { get; set; }
            public string currency_from { get; set; }
            public object date { get; set; }
            public string currency_to { get; set; }
        }

        public class Data
        {
            public string date { get; set; }
            public float sum_result { get; set; }
            public float rate1 { get; set; }
            public float rate2 { get; set; }
        }

    }
}
