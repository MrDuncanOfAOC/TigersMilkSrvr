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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DocumentSearchController : ApiController
    {
        // GET: api/DocumentSearch
        public IEnumerable<Document> Get([FromUri]string term, [FromUri]string id)
        {
            return DocumentHelper.searchDocs(id, term);

        }

        // GET: api/DocumentSearch/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DocumentSearch
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DocumentSearch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DocumentSearch/5
        public void Delete(int id)
        {
        }
    }
}
