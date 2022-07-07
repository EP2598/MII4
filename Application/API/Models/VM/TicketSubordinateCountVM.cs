using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.VM
{
    public class TicketSubordinateCountVM
    {
        public List<string> EmployeeName { get; set; }
        public List<int> TicketCount { get; set; }
    }
}
