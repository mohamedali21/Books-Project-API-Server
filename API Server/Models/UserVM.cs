using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class UserVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Point { get; set; }
        public string IsActive { get; set; }
    }
}