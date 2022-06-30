using API.Models;
using Client.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class AccountRoleRepository : GeneralRepository<AccountRole, string>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountRoleRepository(Address address, string request = "AccountRoles/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }

        public async Task<List<Employee>> GetTeamLead()
        {
            List<Employee> list = new List<Employee>();
            using (var response = await httpClient.GetAsync(request + "GetTeamLead/"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<Employee>>(apiResponse);
            }
            return list;
        }
    }
}
