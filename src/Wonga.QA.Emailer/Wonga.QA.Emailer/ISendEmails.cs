using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Wonga.QA.Emailer.Domain;

namespace Wonga.QA.Emailer
{
    public interface ISendEmails
    {
        void SendEmail(List<TestReport> reports, String email, SmtpClient client);
    }
}
