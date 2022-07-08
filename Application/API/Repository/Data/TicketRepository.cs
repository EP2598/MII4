using API.Base;
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
            //validation form
            if (request.TicketCategory == null)
            {
                return -1;
            }
            else if (request.TicketType == null)
            {
                return -2;
            }
            else if (request.Description == null)
            {
                return -3;
            }
            string ticketID = "T-" + GenerateID();
            Ticket ticket = new Ticket
            {
                TicketId = ticketID,
                CustomerId = request.CustomerID,
                TeamLeadId = request.TeamLeadID,
                TicketCategory = request.TicketCategory,
                TicketType = request.TicketType,
                Description = request.Description,
                Status = "In Progress",
                CreatedAt = DateTime.Now
            };

            var result = 0;

            context.Tickets.Add(ticket);
            try
            {
                result = context.SaveChanges();

                EmailService email = new EmailService();

                #region Send notification to Customer
                Customer custObj = context.Customers.Find(request.CustomerID);
                var emailReceiver = custObj.CustomerEmail;
                var emailSubject = "HalpDesk Support Request";
                var emailBody = "<p>Dear Mr/Mrs " + custObj.CustomerName + "</p><br><p>Your request for support has been received. Your ticket number is " + ticketID + ". Please wait while" +
                    " we process this request.</p><br><p>Thank you</p><br><p><small>This email is generated automatically. Please do not reply to this email.</small></p>";
                email.Send(email.EmailSender, emailReceiver, emailSubject, emailBody);
                #endregion

                #region Send notification to Admin
                Employee empObj = (from a in context.Employees
                                   where a.EmployeeEmail == "admin@email.com" || a.EmployeeEmail == "SUPERUSER@SUPERUSER.USER"
                                   select a).FirstOrDefault();
                if (empObj != null)
                {
                    emailReceiver = empObj.EmployeeEmail;
                    emailBody = "<p>Dear Admin</p><br><p>HalpDesk Support has been requested. The ticket number is " + ticketID + ".</p><br><p>Thank you</p>" +
                        "<br><p><small>This email is generated automatically. Please do not reply to this email.</small></p>";
                    email.Send(email.EmailSender, emailReceiver, emailSubject, emailBody);
                } 
                #endregion
            }
            catch 
            {

            }
            
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
                ticket.TicketCategory = item.TicketCategory;
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
            response.TicketCategory = ticketObj.TicketCategory;
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
        public List<TicketViewVM> GetAllTickets()
        {
            List<TicketViewVM> listTicket = new List<TicketViewVM>();

            var tickets = context.Tickets.ToList();

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
                ticket.TicketCategory = item.TicketCategory;
                ticket.TicketType = item.TicketType;
                ticket.Description = item.Description.Length > 31 ? item.Description.Substring(0, 30) + "..." : item.Description;
                ticket.Status = item.Status;
                ticket.CreatedAt = item.CreatedAt;

                listTicket.Add(ticket);
            }

            return listTicket;
        }
        public List<TicketViewVM> GetAllTicketsByFilter(TicketFilterVM objReq)
        {
            List<TicketViewVM> listTicket = new List<TicketViewVM>();

            List<Ticket> tickets = new List<Ticket>();
            if (objReq.TicketCategory == null && objReq.TicketType == null)
            {
                tickets = context.Tickets.Where(x => x.Status == "Solved").ToList();
            } else if (objReq.TicketCategory == null)
            {
                tickets = context.Tickets.Where(x => x.TicketType == objReq.TicketType && x.Status == "Solved").ToList();
            } else if(objReq.TicketType == null)
            {
                tickets = context.Tickets.Where(x => x.TicketCategory == objReq.TicketCategory && x.Status == "Solved").ToList();
            } else
            {
                tickets = context.Tickets.Where(x => x.TicketType == objReq.TicketType && x.TicketCategory == objReq.TicketCategory && x.Status == "Solved").ToList();
            }
 

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
                ticket.TicketCategory = item.TicketCategory;
                ticket.TicketType = item.TicketType;
                ticket.Description = item.Description.Length > 31 ? item.Description.Substring(0, 30) + "..." : item.Description;
                ticket.Status = item.Status;
                ticket.CreatedAt = item.CreatedAt;

                listTicket.Add(ticket);
            }

            return listTicket;
        }

        public ResponseObj GetSystemStatistic()
        {
            ResponseObj objResp = new ResponseObj();

            TicketStatusCountVM obj = new TicketStatusCountVM();

            List<string> statusName = new List<string>();
            List<int> statusCount = new List<int>();

            try
            {
                var req = (from a in context.Tickets
                           group a by a.Status into aType
                           select new
                           {
                               StatusName = aType.Key,
                               StatusCount = aType.Count()
                           });

                foreach (var item in req)
                {
                    statusName.Add(item.StatusName);
                    statusCount.Add(item.StatusCount);
                }

                obj.StatusName = statusName;
                obj.StatusCount = statusCount;

                objResp.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objResp.message = "Success on getting data";
                objResp.data = obj;
            }
            catch (Exception ex)
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "Failed on getting data";
                objResp.data = ex;
            }

            return objResp;
        }
        public ResponseObj GetSubordinateStatistic(string teamLeadId)
        {
            ResponseObj objResp = new ResponseObj();

            TicketSubordinateCountVM obj = new TicketSubordinateCountVM();
            List<string> listName = new List<string>();
            List<int> listCount = new List<int>();

            /* SQL Statement 
             
            SELECT EMP.EmployeeName, EMP.EmployeeId, COUNT(TI.TicketId) AS [Total Ticket] FROM EMPLOYEES EMP
            LEFT OUTER JOIN TICKETS TI ON EMP.EmployeeId = TI.EmployeeId
            WHERE EMP.TeamLeadId = 'Input Team Lead ID'
            GROUP BY EMP.EmployeeName, EMP.EmployeeId
             
             */

            var req = (from emp in context.Employees
                       where emp.TeamLeadId == teamLeadId
                       join ti in context.Tickets
                       on emp.EmployeeId equals ti.EmployeeId into gj
                       from tickets in gj.DefaultIfEmpty()
                       group tickets by emp.EmployeeId into grouped
                       select new
                       {
                           EmployeeId = grouped.Key,
                           TicketCount = grouped.Count(x => x.TicketId != null)
                       }).ToList();

            foreach (var item in req)
            {
                string empName = context.Employees.Find(item.EmployeeId).EmployeeName;
                listName.Add(empName);
                listCount.Add(item.TicketCount);
            }

            obj.EmployeeName = listName;
            obj.TicketCount = listCount;

            objResp.statusCode = Convert.ToInt32(HttpStatusCode.OK);
            objResp.message = "OK";
            objResp.data = obj;

            return objResp;
        }
        public ResponseObj GetPersonalStatistic(TicketOwnerVM objReq) 
        {
            ResponseObj objResp = new ResponseObj();

            TicketStatusCountVM obj = new TicketStatusCountVM();

            List<string> statusName = new List<string>();
            List<int> statusCount = new List<int>();

            Employee empObj = (from a in context.Employees where a.EmployeeId == objReq.AccountId select a).FirstOrDefault();
            Customer custObj = (from a in context.Customers where a.CustomerId == objReq.AccountId select a).FirstOrDefault();

            try
            {
                if (empObj != null)
                {
                    var req = (from a in context.Tickets
                               where a.EmployeeId == empObj.EmployeeId
                               group a by a.Status into aType
                               select new
                               {
                                   StatusName = aType.Key,
                                   StatusCount = aType.Count()
                               });

                    foreach (var item in req)
                    {
                        statusName.Add(item.StatusName);
                        statusCount.Add(item.StatusCount);
                    }
                }
                else
                {
                    var req = (from a in context.Tickets
                               where a.CustomerId == custObj.CustomerId
                               group a by a.Status into aType
                               select new
                               {
                                   StatusName = aType.Key,
                                   StatusCount = aType.Count()
                               });

                    foreach (var item in req)
                    {
                        statusName.Add(item.StatusName);
                        statusCount.Add(item.StatusCount);
                    }
                }                

                obj.StatusName = statusName;
                obj.StatusCount = statusCount;

                objResp.statusCode = Convert.ToInt32(HttpStatusCode.OK);
                objResp.message = "Success on getting data";
                objResp.data = obj;
            }
            catch (Exception ex)
            {
                objResp.statusCode = Convert.ToInt32(HttpStatusCode.BadRequest);
                objResp.message = "Failed on getting data";
                objResp.data = ex;
            }

            return objResp;
        }
        public int UpdateTicket(UpdateTicketVM ticketVM)
        {
            #region Email Services
            EmailService email = new EmailService();
            var emailReceiver = "";
            var emailSubject = "HalpDesk Support Request";
            var emailBody = "";
            #endregion

            var ticket = context.Tickets.Find(ticketVM.TicketId);
            if (ticket == null)
            {
                return -1;
            }
            ticket.Status = ticketVM.Status;
            context.Entry(ticket).State = EntityState.Modified;
            var result = 0;
            try
            {
                result = context.SaveChanges();

                Customer custObj = context.Customers.Find(ticket.CustomerId);
                emailReceiver = custObj.CustomerEmail;
                emailBody = "<p>Dear Mr/Mrs " + custObj.CustomerName + "</p><br><p>Your ticket " + ticket.TicketId + " has been marked as " + ticket.Status + "." +
                "</p><br><p>Thank you</p><br><p><small>This email is generated automatically. Please do not reply to this email.</small></p>";
                email.Send(email.EmailSender, emailReceiver, emailSubject, emailBody);
            }
            catch
            {

            }
            return result;
        }
        public int AssignTicket(AssignTicketVM ticketVM)
        {
            #region Email Services
            EmailService email = new EmailService();
            var emailReceiver = "";
            var emailSubject = "HalpDesk Support Request";
            var emailBody = "";
            #endregion

            var ticket = context.Tickets.Find(ticketVM.TicketId);
            if (ticketVM.TeamLeadId != null)
            {
                ticket.TeamLeadId = ticketVM.TeamLeadId;

                Employee empObj = context.Employees.Find(ticketVM.TeamLeadId);

                if (empObj != null)
                {
                    emailReceiver = empObj.EmployeeEmail;
                    emailBody = "<p>Dear Mr/Mrs " + empObj.EmployeeName + "</p><br><p>New request for support has been received. Your ticket number is " + ticket.TicketId + "." +
                    "</p><br><p>Thank you</p><br><p><small>This email is generated automatically. Please do not reply to this email.</small></p>";
                }
            }
            else if (ticketVM.EmployeeId != null)
            {
                ticket.EmployeeId = ticketVM.EmployeeId;

                Employee empObj = context.Employees.Find(ticketVM.EmployeeId);

                if (empObj != null)
                {
                    emailReceiver = empObj.EmployeeEmail;
                    emailBody = "<p>Dear Mr/Mrs " + empObj.EmployeeName + "</p><br><p>New request for support has been received. Your ticket number is " + ticket.TicketId + "." +
                    "</p><br><p>Thank you</p><br><p><small>This email is generated automatically. Please do not reply to this email.</small></p>";
                }
            }
            context.Entry(ticket).State = EntityState.Modified;
            var result = 0;
            try
            {
                result = context.SaveChanges();
                if (emailReceiver != "")
                {
                    email.Send(email.EmailSender, emailReceiver, emailSubject, emailBody);
                }
            }
            catch
            {
            }
            
            return result;
        }
        public int EscalateTicket(AssignTicketVM ticketVM)
        {
            Ticket ticket = context.Tickets.Find(ticketVM.TicketId);
            ticket.Status = "Request to Escalate";
            ticket.TeamLeadId = "SUPERUSER";
            ticket.EmployeeId = null;

            context.Entry(ticket).State = EntityState.Modified;
            var result = context.SaveChanges();

            Employee employee = context.Employees.Find(ticketVM.TeamLeadId);
            EmailService email = new EmailService();
            var emailBody = employee.EmployeeName + " make a request to escalate ticket with id: " + ticketVM.TicketId;
            email.Send(email.EmailSender, "admin@email.com", "[Request Escalate Ticket " + ticketVM.TicketId + "]", emailBody);

            return result;
        }

        public int UpdateTypeTicket(UpdateTypeVM ticketVM)
        {
            Ticket ticket = context.Tickets.Find(ticketVM.TicketId);
            ticket.TicketCategory = ticketVM.Type;

            context.Entry(ticket).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;
        }
    }
}
