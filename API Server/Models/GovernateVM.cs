﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Server.Models
{
    public class GovernateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> Cities { get; set; }
    }
}