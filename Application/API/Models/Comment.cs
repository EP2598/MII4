using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [JsonObject(IsReference = true)]
    public class Comment
    {
        public int CommentId { get; set; }
        public string TicketId { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public virtual Account Account { get; set; }
        public virtual Ticket Ticket { get; set; }

    }
}
