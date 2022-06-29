using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Account
    {
        public Account()
        {
            AccountRoles = new HashSet<AccountRole>();
            Comments = new HashSet<Comment>();
        }
        public string Id { get; set; }
        public string Password { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
