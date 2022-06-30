using API.Context;
using API.Models;
using API.Models.VM;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class TicketRepository : GeneralRepository<MyContext, Ticket, string>
    {
        private readonly MyContext context;

        public TicketRepository(MyContext context) : base(context)
        {
            this.context = context;
        }

        public int Request(RequestTicketVM request)
        {
            string ticketID = "T-" + GenerateID();
            Ticket ticket = new Ticket
            {
                TicketId = ticketID,
                CustomerId = request.CustomerID,
                TeamLeadId = request.TeamLeadID,
                TicketType = request.TicketType,
                Description = request.Description,
                Status = "In Progress",
                CreatedAt = DateTime.Now
            };

            context.Tickets.Add(ticket);
            var result = context.SaveChanges();
            return result;
        }
        private string GenerateID()
        {
            string count;
            int last_id;
            string new_count;

            count = (from s in context.Tickets
                        orderby s.TicketId
                        select s.TicketId).LastOrDefault();
           
            if (count == null)
            {
                last_id = 1;
            }
            else
            {
                last_id = Convert.ToInt32(count.Substring(count.Length - 4)) + 1;
            }

            if (last_id < 10)
            {
                new_count = "000" + last_id;
            }
            else if (last_id < 100)
            {
                new_count = "00" + last_id;
            }
            else if (last_id < 1000)
            {
                new_count = "0" + last_id;
            }
            else
            {
                new_count = last_id.ToString();
            }

            return new_count;
        }
    }
}
