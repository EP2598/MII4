using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference = true)]
    public class Role
    {
        public Role()
        {
            AccountRoles = new HashSet<AccountRole>();
        }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<AccountRole> AccountRoles { get; set; }
    }
}
