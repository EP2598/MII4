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

        [HttpPost("GetMyTickets")]
        public ActionResult GetMyTickets(TicketOwnerVM request)
        {
            List<TicketViewVM> objResponse = repository.GetTicketsById(request);

            return Ok(objResponse);
        }

        [HttpPost("GetTicketDetails")]
        public ActionResult GetTicketDetails(RequestTicketDetailVM request)
        {
            TicketViewVM objResponse = repository.GetTicketDetails(request);

            return Ok(objResponse);
        }
    }
}
