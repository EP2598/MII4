using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
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
        public virtual Account Account { get; set; }
    }
}
