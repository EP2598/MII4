using API.Models;
using API.Models.VM;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Authorize]
    public class CommentsController : BaseController<Comment, CommentRepository, int>
    {
        private readonly CommentRepository _commentRepository;
        public CommentsController(CommentRepository commentRepository) : base(commentRepository)
        {
            this._commentRepository = commentRepository;
        }

        [HttpPost]
        public async Task<JsonResult> AddComment(AddCommentVM objReq)
        {
            var userId = User.Claims.Where(x => x.Type.Equals("Id")).FirstOrDefault().Value;

            objReq.AccountId = userId;

            var objResp = await _commentRepository.AddComment(objReq);

            return Json(objResp);
        }

        [HttpPost]
        public async Task<JsonResult> EditComment(EditCommentVM objReq)
        {
            var objResp = await _commentRepository.EditComment(objReq);

            return Json(objResp);
        }
    }
}
