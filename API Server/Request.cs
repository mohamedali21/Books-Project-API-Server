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
    
    public partial class Request
    {
        public int Seller_Id { get; set; }
        public int Book_Id { get; set; }
        public int Requester_Id { get; set; }
        public string Message { get; set; }
        public System.DateTime Date { get; set; }
        public string Acctepted { get; set; }
        public int Offer_Points { get; set; }
    
        public virtual Seller_Approved_book Seller_Approved_book { get; set; }
        public virtual User User { get; set; }
    }
}