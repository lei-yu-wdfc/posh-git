using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Wonga.QA.Emailer.Plugins.SendWarningEmail
{
    public class WarningEmailer
    {
        public void SendWarning(String warning,  String sender, SmtpClient client)
        {
            client.Send(sender, ConfigurationManager.AppSettings["Responsible"], "Emailer has an exception", warning);
        }
    }
}
