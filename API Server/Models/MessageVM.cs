using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class MessageVM
    {
        public int Sender_Id { get; set; }
        public int Receiver_Id { get; set; }
        public int Book_Id { get; set; }
        public string Message1 { get; set; }
        public string Type { get; set; }
        public System.DateTime Msg_Date { get; set; }

    }
}