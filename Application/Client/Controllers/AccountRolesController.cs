using API.Models;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class AccountRolesController : BaseController<AccountRole, AccountRoleRepository, string>
    {
        private readonly AccountRoleRepository repository;
        public AccountRolesController(AccountRoleRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<JsonResult> GetTeamLead()
        {
            var result = await repository.GetTeamLead();
            return Json(result);
        }
    }
}
