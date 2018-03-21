using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace TigersMilkSrvr.Models
{
    public class Document
    {
        public string doc_id { get; set; }
        public string product_name { get; set; }
        public string doc_version { get; set; }
        public string lang_descr { get; set; }
        public string doc_type { get; set; }

    }


    
    public class DocumentHelper
    {
       
        public static IEnumerable<Document> searchDocs(string user_id, string filter)
        {
            string sql =
                "select distinct doc_id, product_name, doc_version, lang_descr, doc_type from XXAOC_PORT_USER_DOCUMENTS_VW where user_id = " +
                user_id + " and product_name like '%" + filter + "%' order by doc_type, product_name, lang_descr";

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["aocport"].ToString());
            OracleCommand cmd = conn.CreateCommand();
            List<Document> data = new List<Document>();

            try
            {
                cmd.CommandText = sql;
                conn.Open();

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);


                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Document d = new Document();
                        d.doc_id = dr["doc_id"].ToString();
                        d.doc_type = dr["doc_type"].ToString();
                        d.doc_version = dr["doc_version"].ToString();
                        d.lang_descr = dr["lang_descr"].ToString();
                        d.product_name = dr["product_name"].ToString();

                        data.Add(d);
                    }
                }
            }
            catch (Exception e)
            {
                data = new List<Document>();
                Debug.WriteLine("ERROR During Document search \nERROR DETAILS: " + e.Message + "\n" + e.StackTrace);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }

            return data;
        }


    }
}