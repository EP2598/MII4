using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference = true)]
    public class AccountRole
    {
        public string AccountId { get; set; }
        public int RoleId { get; set; }
        public virtual Account Account { get; set; }
        public virtual Role Role { get; set; }
    }
}
