using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.VM
{
    public class RegisterVM
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string TeamLeadID { get; set; }
        public int RoleID { get; set; }
    }
}
