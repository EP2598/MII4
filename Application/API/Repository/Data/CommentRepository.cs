using API.Base;
using API.Context;
using API.Models;
using API.Models.VM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Data
{
    public class CommentRepository : GeneralRepository<MyContext, Comment, int>
    {
        private readonly MyContext context;

        public CommentRepository(MyContext context) : base(context)
        {
            this.context = context;
        }

        public int AddComment(AddCommentVM commentVM)
        {
            #region Email Services
            EmailService email = new EmailService();
            var emailReceiver = "";
            var emailSubject = "HalpDesk Support Request";
            var emailBody = "";
            #endregion

            if (context.Tickets.Find(commentVM.TicketId) == null)
            {
                return -1;
            }
            else
            {

                Comment comment = new Comment
                {
                    TicketId = commentVM.TicketId,
                    AccountId = commentVM.AccountId,
                    Description = commentVM.Description,
                    CreatedAt = DateTime.Now,
                    IsEdited = false
                };

                context.Comments.Add(comment);
                var result = 0;
                try
                {
                    result = context.SaveChanges();

                    List<string> emailReceivers = new List<string>();
                    List<string> commentAccountId = (from a in context.Comments
                                                     where a.TicketId == commentVM.TicketId
                                                     select a.AccountId).ToList();
                    IEnumerable<string> listAccoundId = commentAccountId.Distinct();


                    foreach (var item in listAccoundId)
                    {
                        Employee empObj = (from a in context.Employees where a.EmployeeId == item select a).FirstOrDefault();
                        Customer custObj = (from a in context.Customers where a.CustomerId == item select a).FirstOrDefault();

                        if (empObj != null)
                        {
                            if (!String.IsNullOrEmpty(empObj.EmployeeEmail))
                            {
                                emailReceivers.Add(empObj.EmployeeEmail);
                            }
                        }
                        else if (custObj != null)
                        {
                            if (!String.IsNullOrEmpty(custObj.CustomerEmail))
                            {
                                emailReceivers.Add(custObj.CustomerEmail);
                            }
                        }
                    }

                    Employee tempEmp = (from a in context.Employees where a.EmployeeId == commentVM.AccountId select a).FirstOrDefault();
                    Customer tempCust = (from a in context.Customers where a.CustomerId == commentVM.AccountId select a).FirstOrDefault();

                    var commenter = tempEmp == null ? tempCust.CustomerName : tempEmp.EmployeeName;

                    emailBody = "<p>" + commenter + " commented on Ticket " + commentVM.TicketId + ".</p><br><br><p> " + commenter + " <br> <p>&emsp;" + commentVM.Description + "</p>" +
                    "</p><br><br><br><p><small>This email is generated automatically. Please do not reply to this email.</small></p>";

                    foreach (var item in emailReceivers)
                    {
                        emailReceiver = item;
                        email.Send(email.EmailSender, emailReceiver, emailSubject, emailBody);
                    }
                }
                catch
                {

                }
                return result;

            }

        }
                public int EditComment(EditCommentVM commentVM)
        {
            var comment = context.Comments.Find(commentVM.CommentId);
            comment.Description = commentVM.Description;
            comment.IsEdited = true;

            context.Entry(comment).State = EntityState.Modified;
            var result = context.SaveChanges();
            return result;

        }
    }
}
