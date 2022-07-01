using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.VM
{
    public class AddCommentVM
    {
        public string TicketId { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
    }
}
