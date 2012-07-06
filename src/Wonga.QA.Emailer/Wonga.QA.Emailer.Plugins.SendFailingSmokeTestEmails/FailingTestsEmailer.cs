using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Wonga.QA.Emailer.Domain;
using Wonga.QA.Emailer.XmlParser;

namespace Wonga.QA.Emailer.Plugins.SendFailingSmokeTestEmails
{
    public class FailingTestsEmailer : ISendEmails
    {
        public void SendEmail(List<TestReport> reports, String receiver,SmtpClient client)
        {
            client.Send(ConfigurationManager.AppSettings["SMTPUsername"], receiver, "Your tests are failed", CreateEmailText(reports, receiver));
        }

        private string CreateEmailText(List<TestReport> reports, String receiver)
        {
            string autor = receiver.Remove(receiver.IndexOf("@"));

            autor = autor.Replace(autor[0].ToString(), autor[0].ToString().ToUpper());
            if (autor.Contains("."))
            {
                autor = autor.Replace(".", " ");
                autor = autor.Replace(autor[autor.IndexOf(" ") + 1].ToString(), autor[autor.IndexOf(" ") + 1].ToString().ToUpper());
            }

            string message = "Hi " + autor + " your tests are fails.\n\n";
            message += "Used environment: SUT - " + TestReport.SUT + ", AUT - " + TestReport.AUT + ".\n\n";

            foreach (TestReport testReport in reports)
            {
                if (testReport.OwnerEmails.Contains(receiver))
                {
                    DateTime date = DateTime.Parse(testReport.StartTime.Remove(9));
                    string time = testReport.StartTime.Remove(0, 11).Remove(8);
                    message += testReport.TestName + ", was failed.\n\n" +
                               "Test was run " + date.ToString("dd MMMM yyyy") + " at " + time + ".\n\n";
                    if (testReport.JIRA != null)
                    {
                        message += "Tiket in JIRA: " + testReport.JIRA + ".\n\n";
                    }
                    message += "Test location: " + testReport.FullTestName + ".\n\n" +
                               "Fail message: \n" + testReport.Message + ".\n\n";
                }
            }
            message += "Please look on your tests.\n\n";
            message += "This email was generated automatically, don't reply on it.\n\n";
            return message;
        }
    }
}
