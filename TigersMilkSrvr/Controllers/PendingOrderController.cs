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
    public class PendingOrderController : ApiController
    {
        // GET: api/PendingOrder
        public IEnumerable<PendingAOCPortOrder> Get([FromUri]string userid)
        {
            return OrderHelper.getPendingAOCPortOrders(int.Parse(userid));
        }

        // GET: api/PendingOrder/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PendingOrder
        public string Post(PendingAOCPortOrder order)
        {
            return OrderHelper.addReorderRecord(order) == true ? "success" : "fail";
        }

        // PUT: api/PendingOrder/5
        public string Put(int id, PendingAOCPortOrder order)
        {
            return OrderHelper.updateReorderRecord(order) == true ? "success" : "fail";
            
        }

        // DELETE: api/PendingOrder/5
        public string Delete(int id)
        {
            return OrderHelper.deleteReorderRecord(id) == true ? "success" : "fail";
        }
    }
}
