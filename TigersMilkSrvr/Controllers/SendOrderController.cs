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
    public class SendOrderController : ApiController
    {
        // GET: api/SendOrder
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/SendOrder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SendOrder
        public string Post(OrderEmail email)
        {
            string val = "started";
            try
            {
                if (OrderEmailHelper.sendOrder(email))
                    val = "Success indicated";
            }
            catch (Exception e)
            {
                val = "Error: " + e.Message;
            }
            
            return val;
        }

        // PUT: api/SendOrder/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SendOrder/5
        public void Delete(int id)
        {
        }
    }
}
