using API.Models;
using API.Models.VM;
using Client.Repositories.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Controllers
{
    public class AdminController : BaseController<Ticket, TicketRepository, string>
    {
        private readonly TicketRepository _ticketRepos;
        public AdminController(TicketRepository ticketRepos) : base(ticketRepos)
        {
            this._ticketRepos = ticketRepos;
        }
        [HttpPost]
        public async Task<JsonResult> UpdateTypeTicket(UpdateTypeVM ticketVM)
        {
            var result = await _ticketRepos.UpdateTypeTicket(ticketVM);
            return Json(result);
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Assign()
        {
            return View();
        }

    }
}
