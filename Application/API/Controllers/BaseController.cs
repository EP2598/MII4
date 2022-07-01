using API.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BaseController<Entity, Repository, Key> : ControllerBase
           where Entity : class
           where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet("{Key}")]
        public ActionResult<Entity> Get(Key key)
        {
            var result = repository.Get(key);
            if (result != null)
            {
                /*return Ok(result);*/
                return StatusCode(200, new { status = HttpStatusCode.OK, result, message = "Data Found!" });
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Not Found!" });
            }
        }
        [HttpGet]
        public ActionResult<Entity> Get()
        {
            var result = repository.Get();
            if (result.Any())
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, message = "Data Not Found!" });
            }
        }



        [HttpPost]
        public ActionResult<Entity> Post(Entity entity)
        {
            var result = repository.Insert(entity);
            if (result < 1)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Inserted Data Failed!" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data inserted successfully" });
            }
        }

        [HttpPut]
        public ActionResult<Entity> Put(Entity entity)
        {
            var result = repository.Update(entity);
            if (result < 1)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Update Data Failed!" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data updated successfully" });
            }
        }

        [HttpDelete("{Key}")]
        public ActionResult<Entity> Delete(Key key)
        {
            var result = repository.Delete(key);
            if (result < 1)
            {
                return StatusCode(400, new { status = HttpStatusCode.BadRequest, message = "Remove Data Failed!" });
            }
            else
            {
                return StatusCode(200, new { status = HttpStatusCode.OK, message = "Data deleted successfully" });
            }
        }


    }
}
