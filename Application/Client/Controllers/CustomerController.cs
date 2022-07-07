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

        [HttpPost]
        public async Task<JsonResult> GetTicketDetails(RequestTicketDetailVM objReq)
        {
            var objResp = await _ticketRepos.GetTicketDetails(objReq);

            return Json(objResp);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllTickets()
        {
            var result = await _ticketRepos.GetAllTickets();
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetAllTicketsByFilter(TicketFilterVM objReq)
        {
            var result = await _ticketRepos.GetAllTicketsByFilter(objReq);
            return Json(result);
        }


        [HttpGet]
        public async Task<JsonResult> GetSystemStatistic()
        {
            var result = await _ticketRepos.GetSystemStatistic();
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetSubordinateStatistic(TicketOwnerVM objReq)
        {
            var result = await _ticketRepos.GetSubordinateStatistic(objReq);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetPersonalStatistic(TicketOwnerVM objReq)
        {
            var result = await _ticketRepos.GetPersonalStatistic(objReq);
            return Json(result);
        }

        [HttpPut]
        public JsonResult UpdateTicket(UpdateTicketVM ticketVM)
        {
            var result = _ticketRepos.UpdateTicket(ticketVM);
            return Json(result);
        }

        [HttpPut]
        public JsonResult AssignTicket(AssignTicketVM ticketVM)
        {
            var result = _ticketRepos.AssignTicket(ticketVM);
            return Json(result);
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

        public IActionResult SolvedTicket()
        {
            return View();
        }
    }
}
