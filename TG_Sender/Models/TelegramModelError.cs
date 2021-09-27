using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG_Sender.Models
{
    class TelegramModelError
    {
        public bool ok { get; set; }
        public int error_code { get; set; }
        public string description { get; set; }
    }
}
