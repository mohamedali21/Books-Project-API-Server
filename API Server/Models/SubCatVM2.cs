using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class SubCatVM2
    {
        public int subId { get; set; }
        public string SubCatName { get; set; }
        public string SubDiscription { get; set; }

        public int catID { get; set; }

    }
}