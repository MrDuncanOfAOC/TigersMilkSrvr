using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;


namespace TigersMilkSrvr.Models
{

    public class Token
    {
        public string userName { get; set; }
        public int userId { get; set; }
        public DateTime issueDtm { get; set; }
    
    }

    public class TokenManager
    {
        public string token { get; set; }

        public static string GenerateAuthToken(Token payload)
        {

            string token;

            try
            {
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                string secret = ConfigurationManager.AppSettings["key"].ToString();
                token = encoder.Encode(payload, secret);

                //string token = JWT.JsonWebToken.Encode(payload, ConfigurationManager.AppSettings["key"].ToString(), JWT.JwtHashAlgorithm.HS256);
            }
            catch (Exception e)
            {

                throw new Exception("Failed to generate token");
            }
            return token;
        }

        public static Token extractPaylod(string _token)
        {
            string jsonPayload = "";
            Token tokenPayload = null;
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                string secret = ConfigurationManager.AppSettings["key"].ToString();

                jsonPayload = decoder.Decode(_token, secret, verify: true);

                tokenPayload = serializer.Deserialize<Token>(jsonPayload);

                //Console.WriteLine(json);
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }
            
            return tokenPayload;
        }

        public bool validateToken(string _token)
        {
            bool verified = false;
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                string secret = ConfigurationManager.AppSettings["key"].ToString();

                string jsonPayload = decoder.Decode(_token, secret, verify: true);
                //string jsonPayload = JWT.JsonWebToken.Decode(_token, ConfigurationManager.AppSettings["key"].ToString());
                verified = true;
            }
            catch (JWT.SignatureVerificationException)
            {
                verified = false;
                Console.WriteLine("Invalid token!");
            }

            return verified;
        }
    }


    public class AuthFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext filterContext)
        {

            if (!IsUserAuthorized(filterContext))
            {
                
                //Log the user activity


                ShowAuthenticationError(filterContext);
                return;
            }
            base.OnAuthorization(filterContext);
        }

        public bool IsUserAuthorized(HttpActionContext actionContext)
        {
            var authHeader = FetchFromHeader(actionContext); //fetch authorization token from header
            if (authHeader != null)
            {
                Token userPayloadToken = TokenManager.extractPaylod(authHeader);
                

                if (userPayloadToken != null)
                {
                    //Make sure the user's account hasn't been disabled in the middle of a session
                    if (UserHelper.ADValidateEnabled(userPayloadToken))
                    {

                        //TODO: Determine whether the user has access to the requested resource

                        //Log the user activity
                        HallMonitor hm = new HallMonitor();

                        hm.UserId = userPayloadToken.userId;
                        hm.Resource =
                            actionContext.Request.RequestUri.Segments[
                                actionContext.Request.RequestUri.Segments.Length - 1];
                        hm.Action = actionContext.Request.Method.Method;

                        hm.LogActivity();

                        #region comment
                        /*
                            For granular authorization     
                            Get the Requested URI (what controller are we accessing)
                            and the Request Method (GET, POST, PUT, DELETE)

                            compare to the user's role and that role's accessibilities 
                            if all is good (1 = 1) return true otherwise fall through and return false
                        */
                        #endregion
                        if (1 == 1)
                            return true;
                    }
                    else
                        return false;
                }

            }
            return false;
        }

        private static void ShowAuthenticationError(HttpActionContext filterContext)
        {
            //var responseDTO = new ResponseDTO()  = { Code = 401, Message = "Unable to access, Please login again" };
            filterContext.Response =
            filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }

        private string FetchFromHeader(HttpActionContext actionContext)
        {
            string requestToken = null;

            var authRequest = actionContext.Request.Headers.Authorization;
            if (authRequest != null)
            {
                requestToken = authRequest.Parameter;
               
            }

            return requestToken;
        }
    }
}