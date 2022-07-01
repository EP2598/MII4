using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.VM
{
    public class TicketViewVM
    {
        public string TicketId { get; set; }
        public string CustomerId { get; set; }
        public string TeamLeadId { get; set;}
        public string EmployeeId { get; set; }
        public string TicketType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public List<int> CommentsOrder { get; set; }
        public List<string> Comments { get; set; }
        
    }
}
