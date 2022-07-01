using API.Models;
using API.Models.VM;
using Client.Controllers;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Repositories.Data
{
    public class CommentRepository : GeneralRepository<Comment, int>
    {
        private readonly Address _address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor _contextAccessor;

        public CommentRepository(Address address, string request = "Comments/") : base(address, request)
        {
            this._address = address;
            this.request = request;
            _contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(_address.link)
            };
        }

        public async Task<ResponseObj> AddComment(AddCommentVM objReq)
        {
            ResponseObj objResp = null;

            StringContent content = new StringContent(JsonConvert.SerializeObject(objReq), Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync(request + "AddComment", content);

            string apiResponse = await result.Content.ReadAsStringAsync();
            objResp = JsonConvert.DeserializeObject<ResponseObj>(apiResponse);

            return objResp;
        }
    }
}
