//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API_Server
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int Id { get; set; }
        public int Sender_Id { get; set; }
        public int Receiver_Id { get; set; }
        public int Book_Id { get; set; }
        public string Message1 { get; set; }
        public System.DateTime Msg_Date { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
