using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Oracle.ManagedDataAccess.Client;

namespace TigersMilkSrvr.Models
{
    public class User
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool AccountSuspended { get; set; }
        public string Token { get; set; }
        
    }


    public class UserHelper
    {
        public static bool ADValidate(User user)
        {
            bool success = false;


            try
            {

                #region "Get the basic user info"
                // set up domain context
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "AOC-resins.com");

                // find a user
                UserPrincipal principal = UserPrincipal.FindByIdentity(ctx, user.UserName);
                DirectoryEntry de = principal.GetUnderlyingObject() as DirectoryEntry;

                
                user.FirstName = principal.GivenName;
                user.LastName = principal.Surname;
                user.EmailAddress = principal.EmailAddress;
                user.AccountSuspended = principal.Enabled.Value;
                #endregion







                #region "Identify if user is member of group"
                /*
                List<GroupPrincipal> result = new List<GroupPrincipal>();

                if (user != null)
                {
                    PrincipalSearchResult<Principal> groups = principal.GetAuthorizationGroups();
                    //iterate over all groups
                    foreach (Principal p in groups)
                    {
                        if (p is GroupPrincipal)
                        {
                            result.Add((GroupPrincipal)p);
                        }
                    }
                    for (int x = 0; x < result.Count; x++)
                    {
                        if (result[x] != null && result[x].ToString().Contains("AOCPort"))
                        {
                            //This will determine whether the user is a member of the AD group authorized to use AOCPort
                            //TODO: determine whether this is really even relevant 

                            //user.groupAccess = true;
                            break;
                        }
                    }

                }*/
                #endregion

                success = true;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            return success;
        }

        public static bool ADValidateEnabled(Token t)
        {
            bool validated = false;
            try
            {
                //Get the Principal Context for AD
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "AOC-resins.com");
                //Get the User Principal
                UserPrincipal principal = UserPrincipal.FindByIdentity(ctx, t.userName);
                //Is that user's account enabled
                validated = principal.Enabled.Value;
            }
            catch (Exception e)
            {
                validated = false;
            }

            if (!validated)
            {
                HallMonitor hm = new HallMonitor();
                hm.UserId = t.userId;
                hm.Resource = "Active Directory";
                hm.Action = "ENABLED";
                hm.LogDescription = "User account has been disabled.  request for resources has been denied.";
                hm.LogActivity();
            }

            return validated;
        }

        public static User Authenticate(string username, string password)
        {
            
            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["aocport"].ToString());
            OracleCommand cmd = conn.CreateCommand();
            User user;

            try
            {
                conn.Open();
                
                string sql = "SELECT U.*, R.role_desc, UT.type_desc " +
                             " FROM " +
                             "   tm_users U, " + 
                             "   tm_roles R, " +
                             "   tm_user_type UT " +
                             "WHERE(1 = 1) " +
                             "      AND UPPER (user_name) = '" + username + "' " +
                             "      AND password = '" + password + "' " +
                             "      AND account_suspended <= 0 " +
                             "      AND U.role = R.role_id " +
                             "      AND R.user_type = UT.type_id";


                cmd.CommandText = sql;


                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow  dr = ds.Tables[0].Rows[0];
                    user = new User();
                    user.UserId = int.Parse(dr["user_id"].ToString());
                    user.UserName = dr["user_name"].ToString();
                    user.FirstName = dr["first_name"].ToString();
                    user.LastName = dr["last_name"].ToString();
                    user.EmailAddress = dr["email_address"].ToString();

                    Token zToken = new Token
                    {
                        userName = user.UserName,
                        userId = user.UserId,
                        issueDtm = DateTime.Now
                    };
                    user.Token = TokenManager.GenerateAuthToken(zToken);

                }
                else
                {
                    throw new Exception("Authentication failed");
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }
            return user;
        }
    }



    public class Role
    {
        public int RoleId { get; set; }
        public string RoleDescription { get; set; }
        
    }

    public class UserType
    {
        public int TypeId { get; set; }
        public string TypeDescription { get; set; }
        public Role UserRole { get; set; }
    }
       
}