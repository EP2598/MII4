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
                var result = context.SaveChanges();
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
