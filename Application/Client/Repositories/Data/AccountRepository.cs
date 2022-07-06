using API.Models;
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
    public class AccountRepository : GeneralRepository<Account, string>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountRepository(Address address, string request = "Accounts/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }

        public async Task<ResponseObj> Auth(LoginVM objReq)
        {
            ResponseObj token = null;

            StringContent content = new StringContent(JsonConvert.SerializeObject(objReq), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(request + "Login", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            token = JsonConvert.DeserializeObject<ResponseObj>(apiResponse);

            return token;
        }

        public async Task<ResponseObj> ForgetPassword(ForgetPasswordVM objReq)
        {
            ResponseObj objResp = null;

            StringContent content = new StringContent(JsonConvert.SerializeObject(objReq), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(request + "ForgetPassword", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            objResp = JsonConvert.DeserializeObject<ResponseObj>(apiResponse);

            return objResp;
        }

        public async Task<ResponseObj> ChangePassword(ChangePasswordVM objReq)
        {
            ResponseObj objResp = null;

            StringContent content = new StringContent(JsonConvert.SerializeObject(objReq), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(request + "ChangePassword", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            objResp = JsonConvert.DeserializeObject<ResponseObj>(apiResponse);

            return objResp;
        }

        public async Task<ResponseObj> Register(RegisterVM registerVM)
        {
            ResponseObj objResp = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(registerVM), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(address.link + request + "Register/", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            objResp = JsonConvert.DeserializeObject<ResponseObj>(apiResponse);

            return objResp;
        }

    }
}
