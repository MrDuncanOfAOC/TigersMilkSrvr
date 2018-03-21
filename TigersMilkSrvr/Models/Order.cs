using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Net;
using System.Web;

namespace TigersMilkSrvr.Models
{
    public class Order
    {
        public string ORDER_LINE_NO { get; set; }
        public string CUST_PO_NUMBER { get; set; }
        public string ORDERED_ITEM { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_ITEM_NUMBER { get; set; }
        public DateTime DELIVERY_DATE { get; set; }
        public string BATCH_NUMBER { get; set; }
        public double ORDERED_QUANTITY { get; set; }
        public string ORDER_QUANTITY_UOM { get; set; }
        public string ORDER_STATUS { get; set; }
        public string DISPLAY_STATUS { get; set; }
        public string FREIGHT_CARRIER_CODE { get; set; }
        public string TRACKING_NUMBER { get; set; }
        public string LOCATION_NAME { get; set; }
        public string BOL_NUMBER { get; set; }
        public string PDS1 { get; set; }
        public string PDS2 { get; set; }
        public string SDS { get; set; }
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
                         "     FROM XXAOC_PORT_ORDERS_VW a " +
                         "  WHERE     1 = 1 " +
                         "         AND user_id = " + user_id + " " +
                         "          AND trunc(a.delivery_date)between SYSDATE and(SYSDATE + 30) " +
                         "   ORDER BY a.DELIVERY_DATE";

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
                    order.ORDERED_QUANTITY = dr["ORDERED_QUANTITY"] != null
                        ? double.Parse(dr["ORDERED_QUANTITY"].ToString())
                        : 0;
                    order.ORDER_LINE_NO = dr["ORDER_LINE_NO"].ToString();
                    order.ORDER_QUANTITY_UOM = dr["ORDER_QUANTITY_UOM"].ToString();
                    order.ORDER_STATUS = dr["ORDER_STATUS"].ToString();
                    order.PDS1 = dr["PDS1"].ToString();
                    order.PDS2 = dr["PDS2"].ToString();
                    order.SDS = dr["SDS"].ToString();
                    order.TRACKING_NUMBER = dr["TRACKING_NUMBER"].ToString();

                    data.Add(order);


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

    }

}