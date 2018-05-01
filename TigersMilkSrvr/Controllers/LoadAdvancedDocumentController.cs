using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Configuration;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TigersMilkSrvr.Models;
using System.Net.Http.Headers;
using TigersMilkSrvr.COA_WS;

namespace TigersMilkSrvr.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoadAdvancedDocumentController : ApiController
    {
        // GET: api/LoadAdvancedDocument
        public HttpResponseMessage Get([FromUri]string docType, [FromUri]string id)
        {
            var res = new HttpResponseMessage();
            byte[] bytes = null;

            switch (docType.ToUpper())
            {
                case "COA":
                    bytes = OrderHelper.getCOA(id);
                    break;
                case "ASN":
                    bytes = OrderHelper.getASN(id);
                    break;
                case "SDS":
                    bytes = OrderHelper.getDataSheet(id);
                    break;
                case "PDS":
                    bytes = OrderHelper.getDataSheet(id);
                    break;
                case "ACK":
                    bytes = OrderHelper.getACKNOWLEDGEMENT(id);
                    break;
            }

            if (bytes != null)
            {
                res.Content = new ByteArrayContent(bytes);

                res.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            }
            else
            {
                res.Content = new StringContent("The " + docType + " you are looking for is not currently available.  Please check back later.");
                res.Content.Headers.ContentType = new MediaTypeHeaderValue("text/text");
            }
            return res;
        }

        // GET: api/LoadAdvancedDocument/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/LoadAdvancedDocument
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/LoadAdvancedDocument/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/LoadAdvancedDocument/5
        public void Delete(int id)
        {
        }
    }
}
