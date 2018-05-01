using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TigersMilkSrvr.AOCEmailService;

namespace TigersMilkSrvr.Models
{
    public class OrderEmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string subject { get; set; }
        public string body { get; set; }

       
    }

    public class OrderEmailHelper
    {
        public static bool sendOrder(OrderEmail email)
        {
            bool success = false;

            try
            {
                Service svc = new AOCEmailService.Service();

                string val = svc.SendMail("", "", email.From, email.Cc, email.Bcc, email.To, email.subject, email.body);

                success = true;

            }
            catch (Exception e)
            {
                success = false;
            }
            
            return success;
        }
        
    }
}