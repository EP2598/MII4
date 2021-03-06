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
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository repository;
        
        public AccountsController(AccountRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route("Register")]
        public ActionResult Register(RegisterVM register)
        {
            var result = repository.Register(register);
            
            if (result == -1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Nama belum terisi!" });
            }
            else if (result == -2)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Email belum terisi!" });
            }
            else if (result == -3)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Team Lead belum terpilih!" });
            }
            else if (result == -4)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Nomor Telepon belum terisi!" });
            }
            else if (result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Registered User Failed!" });
            }
            else if (result == 401)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Email sudah terdaftar" });
            }
            else if (result == 402)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Nomor telepon sudah terdaftar" });
            }
            else if (result == 403)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Email sudah terdaftar. Nomor telepon sudah terdaftar" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Inserted Data Successed!" });
            }
        }

        [HttpPost("Login")]
        public ActionResult Auth(LoginVM obj)
        {
            ResponseObj objResp = repository.doAuth(obj);
            return StatusCode(objResp.statusCode, objResp);
        }

        [HttpPost("ForgetPassword")]
        public ActionResult ForgetPassword(ForgetPasswordVM obj)
        {
            ResponseObj objResp = repository.ForgetPassword(obj);
            return StatusCode(objResp.statusCode, objResp);
        }

        [HttpPost("ChangePassword")]
        public ActionResult ChangePassword(ChangePasswordVM obj)
        {
            ResponseObj objResp = repository.ChangePassword(obj);
            return StatusCode(objResp.statusCode, objResp);
        }
    }
}
