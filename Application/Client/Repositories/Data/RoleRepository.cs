using API.Models;
using API.Models.VM;
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
    public class RoleRepository : GeneralRepository<Role, int>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public RoleRepository(Address address, string request = "Roles/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }
        public async Task<List<GetRoleVM>> GetRole()
        {
            List<GetRoleVM> list = new List<GetRoleVM>();
            using (var response = await httpClient.GetAsync(request + "GetRole/"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<GetRoleVM>>(apiResponse);
            }
            return list;
        }
    }
}
