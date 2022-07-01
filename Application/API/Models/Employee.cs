using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference = true)]
    public class Employee
    {
        public Employee()
        {
            Tickets = new HashSet<Ticket>();
        }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string TeamLeadId { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
