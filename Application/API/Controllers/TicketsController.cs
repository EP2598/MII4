using API.Models;
using API.Models.VM;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
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
    public class TicketsController : BaseController<Ticket, TicketRepository, string>
    {
        private readonly TicketRepository repository;
        public TicketsController(TicketRepository repository) : base(repository)
        {
            this.repository = repository;
        }
        [HttpPost]
        [Route("Request")]
        public ActionResult RequestTicket(RequestTicketVM request)
        {
            var result = repository.Request(request);
            if(result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Request Ticket Failed!", data="" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Request Ticket Success!", data="" });
            }
        }
        [HttpPost("GetTicketDetails")]
        public ActionResult GetTicketDetails(RequestTicketDetailVM request)
        {
            TicketViewVM objResponse = repository.GetTicketDetails(request);

            return Ok(objResponse);
        }
        [HttpGet("GetAllTickets")]
        public ActionResult GetAllTickets()
        {
            List<TicketViewVM> objResponse = repository.GetAllTickets();
            return Ok(objResponse);
        }

        [HttpPost("GetAllTicketsByFilter")]
        public ActionResult GetAllTicketsByFilter(TicketFilterVM objReq)
        {
            List<TicketViewVM> objResponse = repository.GetAllTicketsByFilter(objReq);
            return Ok(objResponse);
        }

        [HttpPut]
        [Route("Assign")]
        public ActionResult AssignTicket(AssignTicketVM ticketVM)
        {
            var result = repository.AssignTicket(ticketVM);
            if (result == -1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Ticket Not Found!", data = "" });
            }
            else if (result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Assign Ticket Failed!", data = "" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Assign Ticket Success!", data = "" });
            }
        }

        [HttpPut("Update")]
        public ActionResult UpdateTicket(UpdateTicketVM ticketVM)
        {
            var result = repository.UpdateTicket(ticketVM);
            if (result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Update Ticket Failed!", data = "" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Update Ticket Success!", data = "" });
            }
        }

        [HttpPost("GetMyTickets")]
        public ActionResult GetMyTickets(TicketOwnerVM request)
        {
            List<TicketViewVM> objResponse = repository.GetTicketsById(request);

            return Ok(objResponse);
        }

        [HttpPost]
        [Route("Escalate")]
        public ActionResult EscalateTicket(AssignTicketVM ticketVM)
        {
            var result = repository.EscalateTicket(ticketVM);
            if (result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Escalate Ticket Failed!", data = "" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Escalate Ticket Success!", data = "" });
            }
        }

        [HttpPost("UpdateType")]
        public ActionResult UpdateTypeTicket(UpdateTypeVM ticketVM)
        {
            var result = repository.UpdateTypeTicket(ticketVM);
            if (result < 1)
            {
                return StatusCode(400, new { statusCode = HttpStatusCode.BadRequest, message = "Update Type Ticket Failed!", data = "" });
            }
            else
            {
                return StatusCode(200, new { statusCode = HttpStatusCode.OK, message = "Update Type Ticket Success!", data = "" });
            }
        }
    }
}