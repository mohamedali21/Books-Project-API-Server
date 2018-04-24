using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class ReviewVM
    {
        public int Buyer_Id { get; set; }
        public string Buyer_Name { get; set; }
        public int Seller_Id { get; set; }
        public string Seller_Name { get; set; }
        public int Book_Id { get; set; }
        public string Book_Name { get; set; }
        public int Rate { get; set; }
        public string Review { get; set; }
        public DateTime Date { get; set; }
    }
}