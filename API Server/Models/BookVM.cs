using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class BookVM
    {
        public int BookID { get; set; }
        public int UserID { get; set; }
        public string User_Name { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public string Discription { get; set; }
        public Nullable<int> CityId { get; set; }
        public string CityName { get; set; }
        public System.DateTime Date_Uploaded { get; set; }
        public Nullable<int> Subcategory_Id { get; set; }
        public string SubCategory { get; set; }
        public virtual List<string> Images { get; set; }
    }
}