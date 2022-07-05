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
    public class TeamLeadController : BaseController<Ticket, TicketRepository, string>
    {
        private readonly TicketRepository _ticketRepos;
        public TeamLeadController(TicketRepository ticketRepos) : base(ticketRepos)
        {
            this._ticketRepos = ticketRepos;
        }

        [HttpPost]
        public async Task<JsonResult> EscalateTicket(AssignTicketVM ticketVM)
        {
            var userId = User.Claims.Where(x => x.Type.Equals("Id")).FirstOrDefault().Value;
            ticketVM.TeamLeadId = userId;
            var objResp = await _ticketRepos.Escalate(ticketVM);
  
            return Json(objResp);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Assign()
        {
            return View();
        }
    }
}
