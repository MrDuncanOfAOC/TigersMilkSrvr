using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;

namespace TigersMilkSrvr.Models
{
    public class ProductAnalyticsItem
    {

        public string location_name { get; set; }
        public string item_name { get; set; }
        public double quantity { get; set; }

    }

    public class ProductAnalytics
    {
       List<ProductAnalyticsItem> data = new List<ProductAnalyticsItem>();


        public IEnumerable<ProductAnalyticsItem> GetSimpleProductData()
        {
            #region SQL string
            string sql =
                " select location_name, " +
                "         ordered_item, " +
                "       sum(ordered_quantity) total_ordered from( " +
                "       SELECT distinct " +
                "              APPS.XXAOC_WF_UTILITY_PKG.sf_get_bulk_item_no(a.ordered_item) ordered_item, " +
                "              case  " +
                "               when a.order_quantity_uom = 'KG' " +
                "                   then a.ordered_quantity * 2.2046 " +
                "                   else a.ordered_quantity " +
                "              end  ordered_quantity, " +
                "              b.location_name " +
                "         FROM logistics.log_om_orders_vw a, " +
                "              oe_order_lines_all x, " +
                "              logistics.om_scheduled_batches_vw sb, " +
                "              (SELECT l.location_name, c.aoc_site_number " +
                "                 FROM aocport.xx_user_location a, " +
                "                      aocport.locations l, " +
                "                      aocport.locations_site_name_ref c " +
                "                WHERE     1 = 1 " +
                "                      AND l.location_id = a.location_id " +
                "                      AND c.location_id = l.location_id) b " +
                "        WHERE     1 = 1 " +
                "              AND x.line_id = a.line_id " +
                "              AND a.line_id = sb.line_id(+) " +
                "              AND x.flow_status_code NOT IN('AWAITING_RETURN',  'BACKORDERED',  'CANCELLED') " +
                "              AND a.aoc_site_number = b.AOC_SITE_NUMBER " +
                "              AND a.aoc_site_number IN(SELECT aoc_site_number " +
                "                                          FROM aocport.locations_site_name_ref " +
                "                                         WHERE location_id IN(SELECT location_id FROM aocport.xx_user_location WHERE user_id = 1) ) " +
                "              AND order_status <> 'CANCELLED' " +
                "              AND TRUNC(a.delivery_date) between TRUNC(to_date('01-OCT-2016')) AND TRUNC(to_date('01-NOV-2016')) " +
                "              AND Upper(a.order_quantity_uom) in ('LB', 'KG') " +
                //"       --Group by b.location_name, a.ordered_item, a.order_quantity_uom " +
                "       order by b.location_name, ordered_item ) " +
                "       where 1 = 1 " +
                //"           --and upper(location_name) = 'ARMSTRONG' " +
                //"           --and upper(location_name) = 'VALDOSTA' " +
                //"           --and location_name = 'Tring-Jonction' " +
                "           Group by location_name, ordered_item " +
                "           order by location_name, ordered_item ";
            /*    
            Group by location_name
            order by location_name      
            */
            #endregion
            
            
            #region connect to Oracle and get data
            OracleConnection ORA = new OracleConnection();
            ORA.ConnectionString = ConfigurationManager.ConnectionStrings["aocport"].ToString(); //;
            OracleCommand OCMD = ORA.CreateCommand();
            ORA.Open();
            OCMD = ORA.CreateCommand();
            OCMD.CommandText = sql;
            OracleDataAdapter da = new OracleDataAdapter(OCMD);
            DataSet ds = new DataSet();
            da.Fill(ds);

            DataTable dt = ds.Tables[0];
            #endregion



            return this.data;
        }

    }
}