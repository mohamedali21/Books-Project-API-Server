using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class AdVM
    {
        public int BookID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string Description { get; set; }
        public System.DateTime Date_Uploaded { get; set; }
        public string SubCatName { get; set; }
        public int Requests { get; set; }
        public string CityName { get; set; }
        public string Img_Url { get; set; }
    }
}