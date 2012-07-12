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
        public void SendEmail(List<TestReport> reports, String receiver, SmtpClient client)
        {
            var message = new MailMessage(ConfigurationManager.AppSettings["SMTPUsername"], receiver)
                            {
                                Subject = "Your tests are failed",
                                Body = CreateEmailText(reports, receiver),
                                IsBodyHtml = true,
                            };
            client.Send(message);
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

            string message = "<h2>" + "Hi " + autor + " your tests are fails.</h2><br>";
            message += "<h3><font color=#009900>" + "Used environment: SUT - " + TestReport.SUT + ", AUT - " + TestReport.AUT + ".</font></h3><br>";

            foreach (TestReport testReport in reports)
            {
                if (testReport.OwnerEmails.Contains(receiver))
                {
                    DateTime date = DateTime.Parse(testReport.StartTime.Remove(9));
                    string time = testReport.StartTime.Remove(0, 11).Remove(8);
                    message += "<h3><font color=#3333ff>" + testReport.TestName + "</font>" + ", was failed.</h3>" +
                        "Test was run " + "<b>" + date.ToString("dd MMMM yyyy") + "</b>" + " at " + "<b>" + time + "</b>" + ".<br><br>";
                    if (testReport.JIRA != null)
                    {
                        message += "<b>" + "Tiket in JIRA: " + "</b>" + "<a href='" + testReport.JIRA + "'>" + testReport.JIRA + "</a>.<br><br>";
                    }
                    message += "Test location: " + testReport.FullTestName + ".<br><br>" +
                               "<b>Fail message:</b> <br>" + "<font color=#ff0000>" + testReport.Message + ".</font><br><br><br>";
                }
            }
            message += "Please look on your tests.<br><br>";
            message += "This email was generated automatically, don't reply on it.<br><br>";
            return message;
        }
    }
}
