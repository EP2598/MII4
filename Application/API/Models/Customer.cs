﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Customer
    {
        public Customer()
        {
            Tickets = new HashSet<Ticket>();
        }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual Account Account { get; set; }
    }
}
