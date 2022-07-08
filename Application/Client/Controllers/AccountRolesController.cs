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

        [HttpPost]
        public async Task<JsonResult> GetEmployees(TicketOwnerVM ownerVM)
        {
            var userId = User.Claims.Where(x => x.Type.Equals("Id")).FirstOrDefault().Value;
            ownerVM.AccountId = userId;
            var result = await repository.GetEmployees(ownerVM);
            return Json(result);
        }
    }
}
