using API.Context;
using API.Models;
using API.Models.VM;
using Microsoft.AspNetCore.Mvc;
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

            Employee empobj = (from a in context.Employees
                               where a.EmployeeId == request.AccountId
                               select a).FirstOrDefault();

            string adminId = "";

            if (empobj != null)
            {
                if (empobj.EmployeeName == "SUPERUSER")
                {
                    adminId = empobj.EmployeeName;
                }
            }

            List<Ticket> tickets = (from ticket in context.Tickets
                                    where (ticket.CustomerId == request.AccountId) || (ticket.EmployeeId == request.AccountId) || (ticket.TeamLeadId == adminId) || (ticket.TeamLeadId == request.AccountId)
                                    select ticket).ToList();


            foreach (var item in tickets)
            {
                List<Comment> comments = (from comment in context.Comments
                                          where comment.TicketId == item.TicketId
                                          select comment).ToList();

                Customer custObj = (from a in context.Customers
                                    where a.CustomerId == item.CustomerId
                                    select a).FirstOrDefault();

                TicketViewVM ticket = new TicketViewVM();
                ticket.TicketId = item.TicketId;
                ticket.CustomerId = item.CustomerId;
                ticket.CustomerName = custObj.CustomerName;
                ticket.CustomerEmail = custObj.CustomerEmail;
                ticket.TeamLeadId = item.TeamLeadId;
                ticket.TeamLeadName = (from a in context.Employees where a.EmployeeId == item.TeamLeadId select a.EmployeeName).FirstOrDefault();
                ticket.EmployeeId = item.EmployeeId;
                ticket.EmployeeName = (from a in context.Employees where a.EmployeeId == item.EmployeeId select a.EmployeeName).FirstOrDefault();
                ticket.TicketType = item.TicketType;
                ticket.Description = item.Description.Length > 31 ? item.Description.Substring(0,30) + "..." : item.Description;
                ticket.Status = item.Status;
                ticket.CreatedAt = item.CreatedAt;

                listTicket.Add(ticket);
            }

            return listTicket;
        }

        public TicketViewVM GetTicketDetails(RequestTicketDetailVM request)
        {
            TicketViewVM response = new TicketViewVM();

            Ticket ticketObj = (from a in context.Tickets
                                where a.TicketId == request.TicketID
                                select a).FirstOrDefault();

            List<Comment> listComment = (from a in context.Comments
                                         where a.TicketId == request.TicketID
                                         select a).ToList();

            List<string> Commentators = new List<string>();
            foreach (var item in listComment)
            {
                Employee empObj = (from a in context.Employees where a.EmployeeId == item.AccountId select a).FirstOrDefault();
                Customer customerObj = (from a in context.Customers where a.CustomerId == item.AccountId select a).FirstOrDefault();

                string commentator = "";
                if (empObj != null)
                {
                    if (empObj.EmployeeName == "SUPERUSER")
                    {
                        commentator = "Admin";
                    }
                    else
                    {
                        commentator = empObj.EmployeeName;
                    }
                }
                else
                {
                    commentator = customerObj.CustomerName;
                }

                Commentators.Add(commentator);
            }

            Customer custObj = (from a in context.Customers
                                where a.CustomerId == ticketObj.CustomerId
                                select a).FirstOrDefault();

            response.TicketId = ticketObj.TicketId;
            response.CustomerId = ticketObj.CustomerId;
            response.CustomerName = custObj.CustomerName;
            response.CustomerEmail = custObj.CustomerEmail;
            response.TeamLeadId = ticketObj.TeamLeadId;
            response.TeamLeadName = (from a in context.Employees where a.EmployeeId == ticketObj.TeamLeadId select a.EmployeeName).FirstOrDefault();
            response.EmployeeId = ticketObj.EmployeeId;
            response.EmployeeName = (from a in context.Employees where a.EmployeeId == ticketObj.EmployeeId select a.EmployeeName).FirstOrDefault();
            response.TicketType = ticketObj.TicketType;
            response.Description = ticketObj.Description;
            response.Status = ticketObj.Status;
            response.CreatedAt = ticketObj.CreatedAt;
            response.CommentOrder = listComment.Select(x => x.CommentId).ToList();
            response.CommentSender = Commentators;
            response.CommentSenderId = listComment.Select(x => x.AccountId).ToList();
            response.CommentBody = listComment.Select(x => x.Description).ToList();
            response.CommentTimestamps = listComment.Select(x => x.CreatedAt).ToList();
            response.CommentIsEdited = listComment.Select(x => x.IsEdited).ToList();

            return response;
        }
    }
}
