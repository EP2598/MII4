using API.Context;
using API.Models;
using API.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public int AssignTicket(AssignTicketVM ticketVM)
        {
            var ticket = context.Tickets.Find(ticketVM.TicketId);
            if(ticket == null)
            {
                return -1;
            }
            if (ticketVM.TeamLeadId != null)
            {
                
                ticket.TeamLeadId = ticketVM.TeamLeadId;

                
            } else if(ticketVM.EmployeeId != null)
            {
                ticket.EmployeeId = ticketVM.EmployeeId;
            }
            context.Entry(ticket).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }
        public int UpdateTicket(UpdateTicketVM ticketVM)
        {
            var ticket = context.Tickets.Find(ticketVM.TicketId);
            if (ticket == null)
            {
                return -1;
            }
            ticket.Status = ticketVM.Status;
            context.Entry(ticket).State = EntityState.Modified;
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

        public List<TicketViewVM> GetTicketsById(TicketOwnerVM request)
        {
            List<TicketViewVM> listTicket = new List<TicketViewVM>();
            List<Ticket> tickets = (from ticket in context.Tickets
                                    where (ticket.CustomerId == request.AccountId) || (ticket.EmployeeId == request.AccountId)
                                    select ticket).ToList();


            foreach (var item in tickets)
            {
                List<Comment> comments = (from comment in context.Comments
                                          where comment.TicketId == item.TicketId
                                          select comment).ToList();

                TicketViewVM ticket = new TicketViewVM();
                ticket.TicketId = item.TicketId;
                ticket.CustomerId = item.CustomerId;
                ticket.TeamLeadId = item.TeamLeadId;
                ticket.EmployeeId = item.EmployeeId;
                ticket.TicketType = item.TicketType;
                ticket.Description = item.Description.Length > 31 ? item.Description.Substring(0,30) + "..." : item.Description;
                ticket.Status = item.Status;
                ticket.CreatedAt = item.CreatedAt;
                ticket.CommentsOrder = comments.Select(x => x.CommentId).ToList();
                ticket.Comments = comments.Select(x => x.Description).ToList();

                listTicket.Add(ticket);
            }

            return listTicket;
        }
    }
}
