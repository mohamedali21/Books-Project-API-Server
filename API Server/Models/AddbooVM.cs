using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
     
    public class AddbooVM
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Points { get; set; }
        public string Discription { get; set; }
        
        public string SubCatogry { get; set; }
        public string Governate { get; set; }
        public string City { get; set; }
        public List<string> Img { get; set; }

    }
}