﻿using API.Models;
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
            if(result < 1)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Registered User Failed!" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Inserted Data Successed!" });
            }
        }
    }
}