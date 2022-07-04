using API.Models;
using API.Models.VM;
using API.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : BaseController<Comment, CommentRepository, int>
    {
        private readonly CommentRepository repository;
        public CommentsController(CommentRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpPost("AddComment")]
        public ActionResult AddComment(AddCommentVM commentVM)
        {
            var result = repository.AddComment(commentVM);
            if (result == -1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Ticket not found!", data = "" });
            }
            else if (result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Add comment failed!", data = "" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Add comment success!", data = "" });
            }
        }

        [HttpPost]
        [Route("Edit")]
        public ActionResult EditComment(EditCommentVM commentVM)
        {
            var result = repository.EditComment(commentVM);
            if (result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Edit Comment Failed!", data = "" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Edit Comment Success!", data = "" });
            }
        }
    }
}
