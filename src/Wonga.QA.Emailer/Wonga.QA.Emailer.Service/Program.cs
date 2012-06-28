using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using Wonga.QA.Emailer.Domain;
using Wonga.QA.Emailer.Plugins.SendFailingSmokeTestEmails;
using Wonga.QA.Emailer.XmlParser;

namespace Wonga.QA.Emailer.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var reader = new ReportReader();
            string fileName = @"C:\Git\v3QA\bin\Wonga.QA.Tests.Report\test-report-20120627-160908.xml";
            List<TestReport> reports = reader.GetTestsReports(fileName);
            List<String> emails = TestReport.GetOwnersEmails(reports);
            var client = new SmtpClient()
                                    {
                                        Host = (ConfigurationManager.AppSettings["SMTPHost"]),
                                        Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]),
                                        EnableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]),
                                        DeliveryMethod = SmtpDeliveryMethod.Network,
                                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUsername"], ConfigurationManager.AppSettings["SMTPPassword"])
                                    };
            var emailer = new FailingSmokeTestsEmailer();

            foreach (string email in emails)
            {
                emailer.SendEmail(reports, email, client);
            }

            Console.ReadLine();

        }
    }
}
