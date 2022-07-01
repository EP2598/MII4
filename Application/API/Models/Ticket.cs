using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference = true)]
    public class Ticket
    {
        public Ticket()
        {
            Comments = new HashSet<Comment>();
        }

        public string TicketId { get; set; }
        public string CustomerId { get; set; }
        public string TeamLeadId { get; set;}
        public string EmployeeId { get; set; }
        public string TicketType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
