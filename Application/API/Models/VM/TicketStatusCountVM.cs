using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.VM
{
    public class TicketStatusCountVM
    {
        public List<string> StatusName { get; set; }
        public List<int> StatusCount { get; set; }
    }
}
