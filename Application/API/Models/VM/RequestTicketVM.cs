using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.VM
{
    public class RequestTicketVM
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string TeamLeadID { get; set; }
        public string TicketCategory { get; set; }
        public string TicketType { get; set; }
        public string Description { get; set; }

    }
}
