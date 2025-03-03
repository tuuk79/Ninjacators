﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class OrderItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string notes { get; set; }
        public int quantity { get; set; }
        public string cost { get; set; }
        public string price { get; set; }
        public string discount { get; set; }
        public Product product { get; set; }
    }
}