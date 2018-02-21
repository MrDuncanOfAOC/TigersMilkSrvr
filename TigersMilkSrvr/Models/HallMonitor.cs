using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace TigersMilkSrvr.Models
{
    public class HallMonitor
    {

        public int UserId { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
        public string LogDescription { get; set; }


        public void LogActivity()
        {
            //do the insert of the log record

            string sql =
                "insert into TM_TRK_LOG (user_id, resource_name, resource_action, description, log_dtm) values " +
                " (" + this.UserId + ", '" + this.Resource + "', '" + this.Action + "', '" + this.LogDescription + "', SYSDATE)";
            OracleConnection CONN =
                   new OracleConnection(ConfigurationManager.ConnectionStrings["aocport"].ToString());
            OracleCommand CMD = CONN.CreateCommand();
            try
            {
               
                CONN.Open();
                CMD.CommandText = sql.ToUpper();

                int rows = CMD.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                CONN.Close();
                CONN.Dispose();
                CMD.Dispose();
            }

        }

    }


}