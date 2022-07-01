using API.Models;
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
    public class AccountRolesController : BaseController<AccountRole, AccountRolesRepository, string>
    {
        private readonly AccountRolesRepository repository;
        public AccountRolesController(AccountRolesRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetTeamLead")]
        public ActionResult GetTeamLead()
        {
            var result = repository.GetTeamLead();
            if(result != null)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Registered User Failed!", data = result });
            }
        }

    }
}