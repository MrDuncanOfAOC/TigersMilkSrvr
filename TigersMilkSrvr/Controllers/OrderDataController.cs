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
    public class OrderDataController : ApiController
    {
        // GET: api/OrderData
        public IEnumerable<Order> Get([FromUri]string userid)
        {
            return OrderHelper.getOrders(int.Parse(userid));
            
        }

        // GET: api/OrderData/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OrderData
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/OrderData/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OrderData/5
        public void Delete(int id)
        {
        }
    }
}
