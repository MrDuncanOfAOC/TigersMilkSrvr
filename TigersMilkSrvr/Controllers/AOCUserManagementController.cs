using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using TigersMilkSrvr.Models;

namespace TigersMilkSrvr.Controllers
{
    [AuthFilter, EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AOCUserManagementController : ApiController
    {
        // GET: api/AOCUserManagement
        //Return List of existing users
        public IEnumerable<string> Get()
        {


            return new string[] { "value1", "value2" };
        }

        // GET: api/AOCUserManagement/5
        //Return a specific user
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/AOCUserManagement
        // Create a new user
        public void Post([FromBody]string value)
        {
            //An HTTP return status of 200 indicates the save was successful
            //An HTTP return status of 401 indicates the request was refused due to lack of authorization for this task
            //An HTTP return status of 500 indicates an error occurred during the insert process and the user was NOT saved (rollbacks occurred etc)

        }

        // PUT: api/AOCUserManagement/5
        //Update a specific user
        public void Put(int id, [FromBody]string value)
        {
            //An HTTP return status of 200 indicates the update was successful
            //An HTTP return status of 401 indicates the request was refused due to lack of authorization for this task
            //An HTTP return status of 500 indicates an error occurred during the update process and the user changes were NOT saved (rollbacks occurred etc)
        }

        // DELETE: api/AOCUserManagement/5
        //Remove a specific user
        public void Delete(int id)
        {
            //An HTTP return status of 200 indicates the deletion of the user successful
            //An HTTP return status of 401 indicates the request was refused due to lack of authorization for this task
            //An HTTP return status of 500 indicates an error occurred during the delete process and the user was NOT deleted (rollbacks occurred etc)
        }
    }
}
