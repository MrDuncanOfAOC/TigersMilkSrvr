using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Ajax.Utilities;
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
        public string Post(PendingAOCPortOrder order, [FromUri]string action)
        {
            //if(order.PENDING_ORDER_HEADER_ID != null && order.PENDING_ORDER_HEADER_ID.Length > 0)
                if(action.IsNullOrWhiteSpace() || !action.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
                    return OrderHelper.updateReorderRecord(order.PENDING_ORDER_HEADER_ID, order) == true ? "success" : "fail";
                else
                    return OrderHelper.deleteReorderRecord(order.PENDING_ORDER_HEADER_ID) == true ? "success" : "fail";
            //else
            //    return OrderHelper.addReorderRecord(order) == true ? "success" : "fail";
        }

        // PUT: api/PendingOrder/5
        public string Put(PendingAOCPortOrder order)
        {
            return OrderHelper.updateReorderRecord(order.PENDING_ORDER_HEADER_ID, order) == true ? "success" : "fail";
            
        }

        // DELETE: api/PendingOrder/5
        public string Delete([FromUri]string pohi)
        {
            return OrderHelper.deleteReorderRecord(pohi) == true ? "success" : "fail";
        }
    }
}
