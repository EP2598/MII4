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
    public class CustomerController : BaseController<Ticket, TicketRepository, string>
    {
        private readonly TicketRepository _ticketRepos;
        public CustomerController(TicketRepository ticketRepos) : base(ticketRepos)
        {
            this._ticketRepos = ticketRepos;
        }

        [HttpPost]
        public async Task<JsonResult> RequestTicket(RequestTicketVM objReq)
        {
            var userId = User.Claims.Where(x => x.Type.Equals("Id")).FirstOrDefault().Value;
            var userEmail = User.Claims.Where(x => x.Type.Equals("Email")).FirstOrDefault().Value;
            var userPhone = User.Claims.Where(x => x.Type.Equals("Phone")).FirstOrDefault().Value;
            var userName = User.Claims.Where(x => x.Type.Equals("Name")).FirstOrDefault().Value;

            objReq.CustomerID = userId;
            objReq.CustomerEmail = userEmail;
            objReq.CustomerPhone = userPhone;
            objReq.CustomerName = userName;
            objReq.TeamLeadID = "SUPERUSER";

            var objResp = await _ticketRepos.Request(objReq);

            return Json(objResp);
        }

        [HttpPost]
        public async Task<JsonResult> GetMyTickets(TicketOwnerVM objReq)
        {
            var userId = User.Claims.Where(x => x.Type.Equals("Id")).FirstOrDefault().Value;

            objReq.AccountId = userId;

            var objResp = await _ticketRepos.GetMyTickets(objReq);

            return Json(objResp);
        }

        public IActionResult MyTicket()
        {
            return View();
        }
        public IActionResult RequestTicket()
        {
            return View();
        }
        public IActionResult TicketDetails()
        {
            return View();
        }
    }
}
