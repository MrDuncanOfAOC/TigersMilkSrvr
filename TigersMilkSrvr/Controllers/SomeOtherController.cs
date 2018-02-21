using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TigersMilkSrvr.Models;

namespace TigersMilkSrvr.Controllers
{
    [AuthFilter, EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SomeOtherController : ApiController
    {
        // GET: api/SomeOther
        public IEnumerable<string> Get()
        {
            return new string[] { "Access", "successful" };
        }

        // GET: api/SomeOther/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SomeOther
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/SomeOther/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SomeOther/5
        public void Delete(int id)
        {
        }
    }
}
