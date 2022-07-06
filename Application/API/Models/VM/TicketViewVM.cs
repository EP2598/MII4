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
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string TeamLeadId { get; set;}
        public string TeamLeadName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string TicketCategory { get; set; }
        public string TicketType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public List<int> CommentOrder { get; set; }
        public List<string> CommentSender { get; set; }
        public List<string> CommentSenderId { get; set; }
        public List<string> CommentBody { get; set; }
        public List<System.DateTime> CommentTimestamps { get; set; }
        public List<bool> CommentIsEdited { get; set; }
        
    }
}
