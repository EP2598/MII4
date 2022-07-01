﻿using API.Models;
using API.Models.VM;
using Client.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class TicketRepository : GeneralRepository<Ticket, string>
    {
        private readonly Address _address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public TicketRepository(Address address, string request = "Tickets/") : base(address, request)
        {
            this._address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(_address.link)
            };
        }

        public async Task<ResponseObj> Request(RequestTicketVM objReq)
        {
            ResponseObj objResp = null;

            StringContent content = new StringContent(JsonConvert.SerializeObject(objReq), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(request + "Request", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            objResp = JsonConvert.DeserializeObject<ResponseObj>(apiResponse);

            return objResp;
        }

        public async Task<List<TicketViewVM>> GetMyTickets(TicketOwnerVM objReq)
        {
            List<TicketViewVM> objResp;

            StringContent content = new StringContent(JsonConvert.SerializeObject(objReq), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(request + "GetMyTickets", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            objResp = JsonConvert.DeserializeObject<List<TicketViewVM>>(apiResponse);
            
            return objResp;
        }
    }
}
