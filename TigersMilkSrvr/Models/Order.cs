using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters;
using System.Web;
using TigersMilkSrvr.COA_WS;

namespace TigersMilkSrvr.Models
{
    public class Order
    {
        public string AOC_SITE_NUMBER { get; set; }
        public string CUSTOMER_GROUP_CODE { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string ORDER_NUMBER { get; set; }
        public string ORDER_LINE_NO { get; set; }
        public string CUST_PO_NUMBER { get; set; }
        public string ORDERED_ITEM { get; set; }
        public string PRODUCTFAMILY { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_ITEM_NUMBER { get; set; }
        public DateTime DELIVERY_DATE { get; set; }
        public string BATCH_NUMBER { get; set; }
        public double ORDERED_QUANTITY { get; set; }
        public string ORDER_QUANTITY_UOM { get; set; }
        public string ORDER_QUANTITY_LBS { get; set; }
        public string ORDER_QUANTITY_KGS { get; set; }
        public string CSRRESP_EMAIL { get; set; }
        public string ORDERED_CONTAINERS { get; set; }
        public string PACKAGE_CODE { get; set; }
        public string ORDER_STATUS { get; set; }
        public string LINE_STATUS { get; set; }
        public string DISPLAY_STATUS { get; set; }
        public string FREIGHT_CARRIER_CODE { get; set; }
        public string TRACKING_NUMBER { get; set; }
        public string LOCATION_NAME { get; set; }
        public string BOL_NUMBER { get; set; }
        public string PDS { get; set; }
        //public string PDS2 { get; set; }
        public string SDS { get; set; }
    }

    public class PendingAOCPortOrder
    {
        //public string PENDING_ORDER_ID { get; set; }
        public string AOC_SITE_NUMBER { get; set; }
        public string CUSTOMER_GROUP_CODE { get; set; }
        public string CUSTOMER_ID { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_PO_NUMBER { get; set; }
        //public string LINE_SPECIAL_INSTRUCTIONS { get; set; }
        public string LOCATION_NAME { get; set; }
        //public string ORDER_QUANTITY { get; set; }
        public string ORDER_QUANTITY_UOM { get; set; }
        public string ORDER_SPECIAL_INSTRUCTIONS { get; set; }
        //public string ORDERED_CONTAINERS { get; set; }
        public string ORDERED_ITEM { get; set; }
        public string PENDING_ORDER_HEADER_ID { get; set; }
        //public string PACKAGE_CODE { get; set; }
        public string SUBMISSION_STATUS { get; set; }
        //public DateTime REQUESTED_DELIVERY_DATE { get; set; }
        public DateTime SUBMIT_DATE { get; set; }
        public string SUBMITTED_BY_EMAIL_ADDRESS { get; set; }
        public string SUBMITTED_BY_NAME { get; set; }
        public PendingAOCPortOrderLine[] LINES;
    }

    public class PendingAOCPortOrderLine
    {
        //public string PRODUCT_FAMILY { get; set; }
        public string ORDERED_ITEM { get; set; }
        public string ORDERED_QUANTITY { get; set; }
        public string ORDER_QUANTITY_UOM { get; set; }
        public DateTime DELIVERY_DATE { get; set; }
        public string SPECIAL_INSTRUCTIONS { get; set; }

    }

    public class OrderHelper
    {
        List<Order> data = new List<Order>();

        public static IEnumerable<Order> getOrders(int user_id)
        {
            List<Order> data = new List<Order>();

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["aocport"].ToString());
            OracleCommand cmd = conn.CreateCommand();
            string sql = "SELECT a.* " +
                         "     FROM XXAOC_PORT_ORDERS_VW2 a " +
                         "  WHERE     1 = 1 " +
                         "         AND user_id = " + user_id + " " +
                         "          AND trunc(a.delivery_date)between (SYSDATE - 180) and (SYSDATE + 90) " +
                         "   ORDER BY a.DELIVERY_DATE";

            int counter = 0;

            try
            {
                conn.Open();
                cmd.CommandText = sql;

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                DataSet ds = new DataSet();
                da.Fill(ds);

                

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Order order = new Order();

                    order.AOC_SITE_NUMBER = dr["AOC_SITE_NUMBER"].ToString();
                    order.CUSTOMER_GROUP_CODE = dr["CUSTOMER_GROUP_CODE"].ToString();
                    order.CUSTOMER_ID = dr["CUSTOMER_ID"].ToString();
                    order.BATCH_NUMBER = dr["batch_number"].ToString();
                    order.BOL_NUMBER = dr["bol_number"].ToString();
                    order.CUSTOMER_ITEM_NUMBER = dr["CUSTOMER_ITEM_NUMBER"].ToString();
                    order.CUSTOMER_NAME = dr["CUSTOMER_NAME"].ToString();
                    order.CUST_PO_NUMBER = dr["CUST_PO_NUMBER"].ToString();
                    order.DELIVERY_DATE = (DateTime) dr["DELIVERY_DATE"];
                    order.DISPLAY_STATUS = dr["DISPLAY_STATUS"].ToString();
                    order.FREIGHT_CARRIER_CODE = dr["FREIGHT_CARRIER_CODE"].ToString();
                    order.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                    order.ORDERED_ITEM = dr["ORDERED_ITEM"].ToString();
                    order.PRODUCTFAMILY = dr["PRODUCTFAMILY"].ToString();
                    order.ORDERED_QUANTITY = (dr["ORDERED_QUANTITY"] != null && dr["ORDERED_QUANTITY"].ToString().Trim().Length > 0)
                        ? double.Parse(dr["ORDERED_QUANTITY"].ToString())
                        : 0;
                    order.ORDER_NUMBER = dr["ORDER_NUMBER"].ToString();
                    order.ORDER_LINE_NO = dr["ORDER_LINE_NO"].ToString();
                    order.ORDER_QUANTITY_UOM = dr["ORDER_QUANTITY_UOM"].ToString();
                    order.ORDER_STATUS = dr["ORDER_STATUS"].ToString();
                    order.LINE_STATUS = dr["LINE_STATUS"].ToString();
                    order.PDS = dr["PDS"].ToString();
                    //order.PDS2 = dr["PDS2"].ToString();
                    order.SDS = dr["SDS"].ToString();
                    order.TRACKING_NUMBER = dr["TRACKING_NUMBER"].ToString();
                    order.ORDER_QUANTITY_LBS = dr["ORDER_QUANTITY_LBS"].ToString();
                    order.ORDER_QUANTITY_KGS = dr["ORDER_QUANTITY_KGS"].ToString();
                    order.CSRRESP_EMAIL = dr["CSRRESP_EMAIL"].ToString();
                    order.ORDERED_CONTAINERS = dr["ORDERED_CONTAINERS"].ToString();
                    order.PACKAGE_CODE = dr["PACKAGE_CODE"].ToString();
                    

                    data.Add(order);
                    counter++;

                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: " + e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }



            return data;

        }

        public static IEnumerable<PendingAOCPortOrder> getPendingAOCPortOrders(int user_id)
        {
            List<PendingAOCPortOrder> data = new List<PendingAOCPortOrder>();

            OracleConnection conn = new OracleConnection(ConfigurationManager.ConnectionStrings["aocport"].ToString());
            OracleCommand cmd = conn.CreateCommand();
            string sql = "SELECT * " +
                         "     FROM AOCPORT_PENDING_ORDERS " +
                         "  WHERE     1 = 1 " +
                         //"         AND user_id = " + user_id + " " +
                         "   ORDER BY PENDING_ORDER_HEADER_ID, REQUESTED_DELIVERY_DATE";

            int counter = 0;

            try
            {
                conn.Open();
                cmd.CommandText = sql;

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];

                    DataView view = new DataView(dt);
                    DataTable dtOrderHeaderId = view.ToTable(true, "PENDING_ORDER_HEADER_ID");

                    foreach (DataRow dr in dtOrderHeaderId.Rows)
                    {
                        //Filter to rows that match the current PENDING_ORDER_HEADER_ID
                        DataRow[] rows =  dt.Select("PENDING_ORDER_HEADER_ID = '" + dr["PENDING_ORDER_HEADER_ID"].ToString() + "' ");


                        PendingAOCPortOrder order = new PendingAOCPortOrder();
                        


                        order.PENDING_ORDER_HEADER_ID = dr["PENDING_ORDER_HEADER_ID"].ToString();
                        //order.PENDING_ORDER_ID = rows[0]["PENDING_ORDER_ID"].ToString();
                        order.AOC_SITE_NUMBER = rows[0]["AOC_SITE_NUMBER"].ToString();
                        order.CUSTOMER_GROUP_CODE = rows[0]["CUSTOMER_GROUP_CODE"].ToString();
                        order.CUSTOMER_ID = rows[0]["CUSTOMER_ID"].ToString();
                        order.CUSTOMER_NAME = rows[0]["CUSTOMER_NAME"].ToString();
                        order.CUSTOMER_PO_NUMBER = rows[0]["CUSTOMER_PO_NUMBER"].ToString();
                        order.LOCATION_NAME = rows[0]["LOCATION_NAME"].ToString();
                        order.ORDERED_ITEM = rows[0]["ORDERED_ITEM"].ToString();
                        order.ORDER_QUANTITY_UOM = rows[0]["ORDER_QUANTITY_UOM"].ToString();
                        order.ORDER_SPECIAL_INSTRUCTIONS = rows[0]["ORDER_SPECIAL_INSTRUCTIONS"].ToString();
                        order.SUBMIT_DATE = (DateTime)rows[0]["SUBMIT_DATE"];
                        order.SUBMITTED_BY_EMAIL_ADDRESS = rows[0]["SUBMITTED_BY_EMAIL_ADDRESS"].ToString();
                        order.SUBMITTED_BY_NAME = rows[0]["SUBMITTED_BY_NAME"].ToString();
                        order.SUBMISSION_STATUS = rows[0]["SUBMISSION_STATUS"].ToString();
                        order.LINES = new PendingAOCPortOrderLine[rows.Length];

                        for (int l = 0; l < rows.Length; l++)
                        {
                            PendingAOCPortOrderLine line = new PendingAOCPortOrderLine();

                            line.DELIVERY_DATE = (DateTime)rows[l]["REQUESTED_DELIVERY_DATE"];
                            line.ORDERED_ITEM = rows[l]["ORDERED_ITEM"].ToString();
                            line.ORDERED_QUANTITY = rows[l]["ORDER_QUANTITY"].ToString();
                            line.ORDER_QUANTITY_UOM = rows[l]["ORDER_QUANTITY_UOM"].ToString();
                            line.SPECIAL_INSTRUCTIONS = rows[l]["LINE_SPECIAL_INSTRUCTIONS"].ToString();

                            order.LINES[l] = line;


                        }




                        
                        /*order.ORDERED_CONTAINERS = dr["ORDERED_CONTAINERS"].ToString();
                        order.PACKAGE_CODE = dr["PACKAGE_CODE"].ToString();*/
                        

                        data.Add(order);

                    }

                    /*foreach (DataRow dr in dt.Rows)
                    {
                       

                        order.PENDING_ORDER_HEADER_ID = dr["PENDING_ORDER_HEADER_ID"].ToString();
                        order.PENDING_ORDER_ID = dr["PENDING_ORDER_ID"].ToString();
                        order.AOC_SITE_NUMBER = dr["AOC_SITE_NUMBER"].ToString();
                        order.CUSTOMER_GROUP_CODE = dr["CUSTOMER_GROUP_CODE"].ToString();
                        order.CUSTOMER_ID = dr["CUSTOMER_ID"].ToString();
                        order.CUSTOMER_NAME = dr["CUSTOMER_NAME"].ToString();
                        order.CUSTOMER_PO_NUMBER = dr["CUSTOMER_PO_NUMBER"].ToString();
                        order.LINE_SPECIAL_INSTRUCTIONS = dr["LINE_SPECIAL_INSTRUCTIONS"].ToString();
                        order.LOCATION_NAME = dr["LOCATION_NAME"].ToString();
                        order.ORDER_QUANTITY = dr["ORDER_QUANTITY"].ToString();
                        order.ORDER_QUANTITY_UOM = dr["ORDER_QUANTITY_UOM"].ToString();
                        order.ORDER_SPECIAL_INSTRUCTIONS = dr["ORDER_SPECIAL_INSTRUCTIONS"].ToString();
                        order.ORDERED_ITEM = dr["ORDERED_ITEM"].ToString();
                        order.ORDERED_CONTAINERS = dr["ORDERED_CONTAINERS"].ToString();
                        order.PACKAGE_CODE = dr["PACKAGE_CODE"].ToString();
                        order.SUBMISSION_STATUS = dr["SUBMISSION_STATUS"].ToString();
                        order.REQUESTED_DELIVERY_DATE = (DateTime) dr["REQUESTED_DELIVERY_DATE"];
                        order.SUBMIT_DATE = (DateTime) dr["SUBMIT_DATE"];
                        order.SUBMITTED_BY_EMAIL_ADDRESS = dr["SUBMITTED_BY_EMAIL_ADDRESS"].ToString();
                        order.SUBMITTED_BY_NAME = dr["SUBMITTED_BY_NAME"].ToString();


                       
                        counter++;

                    }*/
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("ERROR: " + e.Message);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
                cmd.Dispose();
            }



            return data;

        }

        public static bool addReorderRecord(PendingAOCPortOrder order)
        {
            bool success = false;

            OracleConnection ORA = new OracleConnection();
            ORA.ConnectionString = ConfigurationManager.ConnectionStrings["aocport"].ToString();
            OracleCommand OCMD = ORA.CreateCommand();


            #region SQL Statement
            string[] sql = new string[order.LINES.Length];
            for (int L = 0; L < order.LINES.Length; L++)
            {
               sql[L] =  "insert into AOCPORT_PENDING_ORDERS (" +
                       "PENDING_ORDER_HEADER_ID, " +
                       "AOC_SITE_NUMBER, " +
                       "CUSTOMER_GROUP_CODE, " +
                       "CUSTOMER_ID, " +
                       "CUSTOMER_NAME, " +
                       "CUSTOMER_PO_NUMBER, " +
                       "LINE_SPECIAL_INSTRUCTIONS, " +
                       "LOCATION_NAME, " +
                       //"ORDERED_CONTAINERS, " +
                       "ORDERED_ITEM, " +
                       "ORDER_QUANTITY, " +
                       "ORDER_QUANTITY_UOM, " +
                       "ORDER_SPECIAL_INSTRUCTIONS, " +
                       //"PACKAGE_CODE, " +
                       "SUBMISSION_STATUS, " +
                       "REQUESTED_DELIVERY_DATE, " +
                       "SUBMITTED_BY_EMAIL_ADDRESS, " +
                       "SUBMITTED_BY_NAME, " +
                       "SUBMIT_DATE) values ( " +
                       "'" + order.PENDING_ORDER_HEADER_ID + "', " +
                       "'" + order.AOC_SITE_NUMBER + "', " +
                       order.CUSTOMER_GROUP_CODE + ", " +
                       order.CUSTOMER_ID + ", " +
                       "'" + order.CUSTOMER_NAME + "', " +
                       "'" + order.CUSTOMER_PO_NUMBER + "', " +
                       "'" + order.LINES[L].SPECIAL_INSTRUCTIONS + "', " +
                       "'" + order.LOCATION_NAME + "', " +
                       //order.LINES[0].ORDERED_CONTAINERS + ", " +
                       "'" + order.ORDERED_ITEM + "', " +
                       order.LINES[L].ORDERED_QUANTITY + ", " +
                       "'" + order.LINES[L].ORDER_QUANTITY_UOM + "', " +
                       "'" + order.ORDER_SPECIAL_INSTRUCTIONS + "', " +
                       //"'" + order.PACKAGE_CODE + "', " +
                       "'" + order.SUBMISSION_STATUS + "', " +
                       "TO_DATE('" + order.LINES[L].DELIVERY_DATE.ToString("dd-MMM-yyyy") + "'), " +
                       "'" + order.SUBMITTED_BY_EMAIL_ADDRESS + "', " +
                       "'" + order.SUBMITTED_BY_NAME + "', " +
                       "TO_DATE('" + order.SUBMIT_DATE.ToString("dd-MMM-yyyy") + "') " +
                       ") ";

            }
              
            #endregion


            ORA.Open();
            OCMD = ORA.CreateCommand();
            OracleTransaction txn = ORA.BeginTransaction();


            #region Inser the new reorder record
            try
            {
                OCMD.Transaction = txn;

                for (int L = 0; L < order.LINES.Length; L++)
                {
                    
                   
                    OCMD.CommandText = sql[L];

                    int recs = OCMD.ExecuteNonQuery();

                    

                }
                txn.Commit();

                success = true;


            }
            catch (Exception e)
            {
                txn.Rollback();
                success = false;
            }
            finally
            {
                ORA.Dispose();
                OCMD.Dispose();
            }
            #endregion

            int meaningless = 0;

            return success;
        }

        public static bool deleteReorderRecord(string pohi)
        {
            bool success = false;

            OracleConnection ORA = new OracleConnection();
            ORA.ConnectionString = ConfigurationManager.ConnectionStrings["aocport"].ToString();
            OracleCommand OCMD = ORA.CreateCommand();


            #region SQL Statement
            string sql = "delete from AOCPORT_PENDING_ORDERS  where 1 = 1 " +
                         "AND PENDING_ORDER_HEADER_ID = '" + pohi + "' ";
            #endregion


            ORA.Open();
            OCMD = ORA.CreateCommand();

            OracleTransaction txn = ORA.BeginTransaction();
            OCMD.Transaction = txn;

            #region Inser the new reorder record
            try
            {
                OCMD.CommandText = sql;

                int recs = OCMD.ExecuteNonQuery();

                txn.Commit();

                success = true;


            }
            catch (Exception e)
            {
                txn.Rollback();
                success = false;
            }
            finally
            {
                ORA.Dispose();
                OCMD.Dispose();
            }
            #endregion

            int meaningless = 0;

            return success;
        }


        public static bool updateReorderRecord(string pohi, PendingAOCPortOrder order)
        {
            bool success = false;


            success = (deleteReorderRecord(pohi) && addReorderRecord(order));

            #region old code
            /*OracleConnection ORA = new OracleConnection();
            ORA.ConnectionString = ConfigurationManager.ConnectionStrings["aocport"].ToString();
            OracleCommand OCMD = ORA.CreateCommand();


            #region SQL Statement
            string sql = "UPDATE AOCPORT_PENDING_ORDERS SET " +
                         "AOC_SITE_NUMBER = " + "'" + order.AOC_SITE_NUMBER + "', " +
                         "CUSTOMER_GROUP_CODE = " + order.CUSTOMER_GROUP_CODE + ", " +
                         "CUSTOMER_ID = " + order.CUSTOMER_ID + ", " +
                         "CUSTOMER_NAME = " + "'" + order.CUSTOMER_NAME + "', " +
                         "CUSTOMER_PO_NUMBER = " + "'" + order.CUSTOMER_PO_NUMBER + "', " +
                         "LINE_SPECIAL_INSTRUCTIONS = " + "'" + order.LINE_SPECIAL_INSTRUCTIONS + "', " +
                         "LOCATION_NAME = " + "'" + order.LOCATION_NAME + "', " +
                         "ORDERED_CONTAINERS = " + order.ORDERED_CONTAINERS + ", " +
                         "ORDERED_ITEM = " + "'" + order.ORDERED_ITEM + "', " +
                         "ORDER_QUANTITY = " + order.ORDER_QUANTITY + ", " +
                         "ORDER_QUANTITY_UOM = " + "'" + order.ORDER_QUANTITY_UOM + "', " +
                         "ORDER_SPECIAL_INSTRUCTIONS = " + "'" + order.ORDER_SPECIAL_INSTRUCTIONS + "', " +
                         "PACKAGE_CODE = " + "'" + order.PACKAGE_CODE + "', " +
                         "SUBMISSION_STATUS = " + "'" + order.SUBMISSION_STATUS + "', " +
                         "REQUESTED_DELIVERY_DATE = " + "TO_DATE('" + order.REQUESTED_DELIVERY_DATE.ToString("dd-MMM-yyyy") + "'), " +
                         "SUBMITTED_BY_EMAIL_ADDRESS = " + "'" + order.SUBMITTED_BY_EMAIL_ADDRESS + "', " +
                         "SUBMITTED_BY_NAME = " + "'" + order.SUBMITTED_BY_NAME + "', " +
                         "SUBMIT_DATE = TO_DATE('" + order.SUBMIT_DATE.ToString("dd-MMM-yyyy") + "') " +
                         "WHERE PENDING_ORDER_HEADER_ID = '" + order.PENDING_ORDER_HEADER_ID + "' AND PENDING_ORDER_ID = " + order.PENDING_ORDER_ID;







            #endregion


            ORA.Open();
            OCMD = ORA.CreateCommand();

            OracleTransaction txn = ORA.BeginTransaction();
            OCMD.Transaction = txn;

            #region Inser the new reorder record
            try
            {
                OCMD.CommandText = sql;

                int recs = OCMD.ExecuteNonQuery();

                txn.Commit();

                success = true;


            }
            catch (Exception e)
            {
                txn.Rollback();
                success = false;
            }
            finally
            {
                ORA.Dispose();
                OCMD.Dispose();
            }*/
            #endregion

            int meaningless = 0;

            return success;
        }
        public static byte[] getASN(string id)
        {
            byte[] temp = null;
            OracleConnection ORA = new OracleConnection();
            ORA.ConnectionString = ConfigurationManager.ConnectionStrings["aocport"].ToString(); //;
            OracleCommand OCMD = ORA.CreateCommand();

            string oSql = "SELECT file_names from (select MAX(creation_date) creation_date, bol_number, file_names from apps.xxaoc_document_requests where file_names LIKE '%ASN%' AND status = 'C' and bol_number= '" + id + "'  group by bol_number, file_names order by creation_date DESC) where rownum=1";
            OCMD.CommandText = oSql;

            try
            {
                ORA.Open();

                string file_name = OCMD.ExecuteScalar().ToString();
                if (file_name != null && file_name.Length > 0)
                    temp = System.IO.File.ReadAllBytes("//" + ConfigurationManager.AppSettings["ASNPath"] + file_name.Replace("@", "").Replace("\"", "").Trim()); 
                
            }
            catch (Exception e)
            {
                temp = null;
            }
            finally
            {
                ORA.Close();
                ORA.Dispose();
                OCMD.Dispose();
            }

            return temp;
          }


        public static byte[] getACKNOWLEDGEMENT(string id)
        {
            byte[] temp = null;
            OracleConnection ORA = new OracleConnection();
            ORA.ConnectionString = ConfigurationManager.ConnectionStrings["aocport"].ToString(); //;
            OracleCommand OCMD = ORA.CreateCommand();

            string oSql = "select file_names from ( " +
                          "selecT order_ack_ordeR_number, file_names, rank() over(partition by order_ack_ordeR_number order by creation_date desc) rnk, bol_number from xxaoc.XXAOC_DOCUMENT_REQUESTS ) " +
                          "where rnk = 1 and order_ack_ordeR_number = '" + id + "' ";


            OCMD.CommandText = oSql;

            try
            {
                ORA.Open();

                string file_name = OCMD.ExecuteScalar().ToString();
                if (file_name != null && file_name.Length > 0)
                    temp = System.IO.File.ReadAllBytes("//" + ConfigurationManager.AppSettings["ASNPath"] + file_name.Replace("@", "").Replace("\"", "").Trim());

            }
            catch (Exception e)
            {
                temp = null;
            }
            finally
            {
                ORA.Close();
                ORA.Dispose();
                OCMD.Dispose();
            }

            return temp;
        }

        public static byte[] getCOA(string id)
        {
            byte[] temp = null;
            try
            {
                COA_WS.COAReportService COA = new COAReportService();
                string file_name = "";

                COA.GetCoaDocuments(id, out file_name);

                if (file_name != null && file_name.Length > 0)
                    temp =System.IO.File.ReadAllBytes("//" + ConfigurationManager.AppSettings["COAPath"] +
                                                    file_name.Replace("@", "").Replace("\"", "").Trim());
                

            }
            catch (Exception e)
            {
             temp = null;
            }

            return temp;
        }


        public static byte[] getDataSheet(string id)
        {
            byte[] pdf;
            try
            {
                
                String block = " ";
                OracleConnection ORA = new OracleConnection();
                ORA.ConnectionString = ConfigurationManager.ConnectionStrings["aocport"].ToString(); //;
                OracleCommand OCMD = ORA.CreateCommand();
                //Doc_type 1 = PDS Doc_type 2 = SDS
                block = "select doc_file from msds_om.documents_view where doc_id = " + id;

                ORA.Open();
                OCMD = ORA.CreateCommand();
                OCMD.CommandText = block;

                pdf = (byte[])OCMD.ExecuteScalar();


                
            }
            catch (Exception e)
            {
                pdf = null;
            }

            return pdf;
        }
    }

}