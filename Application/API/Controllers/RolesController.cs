using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController<Role, RoleRepository, int>
    {
        private readonly RoleRepository repository;
        public RolesController(RoleRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetRole")]
        public ActionResult GetRole()
        {
            var result = repository.GetRole();
            return Ok(result);
        }
    }
}
