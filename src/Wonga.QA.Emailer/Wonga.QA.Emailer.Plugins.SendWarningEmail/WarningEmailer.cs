using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Wonga.QA.Emailer.Plugins.SendWarningEmail
{
    public class WarningEmailer
    {
        public void SendWarning(String warning, String sender, SmtpClient client)
        {
            String host = Dns.GetHostName();
            IPAddress ip = Dns.GetHostAddresses(host).FirstOrDefault();
            String message = warning + "\n\n" + "Host: " + host + "\n\n" + "Ip addres: " + ip.ToString();
            client.Send(sender, ConfigurationManager.AppSettings["Responsible"], "Emailer has an exception", message);
        }
    }
}
