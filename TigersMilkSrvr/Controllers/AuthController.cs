using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using TigersMilkSrvr.Models;

namespace TigersMilkSrvr.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthController : ApiController
    {

        
        // GET: api/Auth
        public IEnumerable<string> Get([FromUri]string username, [FromUri] string password)
        {
           
            string token = "";

            User u = new User();
            //Get the username

            
            u.UserName = "cduncan";

            if (UserHelper.ADValidate(u))
            {

                Token t = new Token
                {
                    userName = u.UserName,
                    userId = u.UserId,
                    issueDtm = DateTime.Now
                };
                
                u.Token = TokenManager.GenerateAuthToken(t);

                
            }

            Token zToken = TokenManager.extractPaylod(u.Token);
            


            //Authenticate the user from the login screen...
            
            



            return new string[] { "Token: ", u.Token };
        }

        // GET: api/Auth/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Auth
        public User Post([FromBody] Credentials obj)
        {


            try
            {
                return UserHelper.Authenticate(obj.username, obj.password);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
           
            
        }

        // PUT: api/Auth/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Auth/5
        public void Delete(int id)
        {
        }
    }


    public class Credentials
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
