using API.Models;
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
    public class RolesController : BaseController<Role, RoleRepository, int>
    {
        private readonly RoleRepository repository;
        public RolesController(RoleRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<JsonResult> GetRole()
        {
            var result = await repository.GetRole();
            return Json(result);
        }
    }
}
