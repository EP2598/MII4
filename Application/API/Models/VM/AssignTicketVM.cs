using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.VM
{
    public class AssignTicketVM
    {
        public string TicketId { get; set; }
        public string TeamLeadId { get; set; }
        public string EmployeeId { get; set; }
    }
}
