using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class RequestVM
    {
        public int Seller_Id { get; set; }
        public string SellerName{ get; set; }
        public int Requester_Id { get; set; }
        public string RequesterName { get; set; }
        public int Book_Id { get; set; }
        public string BookImgUrl { get; set; }
        public string BookName { get; set; }
        public int Points { get; set; }
        public int Offer_Points { get; set; }
        public string Message { get; set; }
        public DateTime ReqDate { get; set; }


    }
}