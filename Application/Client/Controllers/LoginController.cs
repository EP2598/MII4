using API.Models;
using API.Models.VM;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class LoginController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository _accRepos;
        public LoginController(AccountRepository accRepos) : base(accRepos)
        {
            this._accRepos = accRepos;
        }

        [HttpPost]
        public async Task<JsonResult> Auth(LoginVM objReq)
        {
            var jwtToken = await _accRepos.Auth(objReq);
            var token = jwtToken.data;

            if (token == null)
            {
                HttpContext.Session.SetString("JWToken", "");
            }
            else
            {
                HttpContext.Session.SetString("JWToken", token.ToString());
            }
            

            return Json(jwtToken);
        }

        [HttpPost]
        public async Task<JsonResult> ForgotPassword(ForgetPasswordVM objReq)
        {
            var objResp = await _accRepos.ForgetPassword(objReq);

            return Json(objResp);
        }

        [HttpPost]
        public async Task<JsonResult> SubmitChangePassword(ChangePasswordVM objReq)
        {
            var objResp = await _accRepos.ChangePassword(objReq);

            return Json(objResp);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
