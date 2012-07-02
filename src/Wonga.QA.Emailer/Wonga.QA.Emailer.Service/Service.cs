using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using Wonga.QA.Emailer.Domain;
using Wonga.QA.Emailer.Plugins.SendFailingSmokeTestEmails;
using Wonga.QA.Emailer.XmlParser;

namespace Wonga.QA.Emailer.Service
{
    public partial class Service : ServiceBase
    {
        private StreamWriter _logFile;
        private Timer _timer;

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logFile = new StreamWriter(new FileStream(ConfigurationManager.AppSettings["LogFilePath"], FileMode.Append));
            _logFile.WriteLine("Wonga.QA.Emailer Service start!");
            _logFile.Flush();
            _timer = new Timer { Enabled = true, Interval = 30000, AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _logFile.WriteLine("Wonga.QA.Emailer Service stop.");
            _logFile.Flush();
            _logFile.Close();
        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            _logFile.WriteLine(DateTime.Now.ToString("Current time: dd MMMM yyyy HH:mm:ss"));

            var folderPath = "@" + ConfigurationManager.AppSettings["TempFolderPath"];
            var reports = Directory.GetFiles(folderPath);
            if (reports.Count() != 0)
            {
                foreach (var report in reports)
                {
                    ParseReportAndSendEmails(report);
                    _logFile.WriteLine("file " + report + "was processed");
                    File.Delete(folderPath + @"\" + report);
                    _logFile.WriteLine("file " + report + "was deleted");
                }
            }

            _logFile.Flush();
        }

        private static void ParseReportAndSendEmails(string fileName)
        {
            var client = new SmtpClient()
                             {
                                 Host = (ConfigurationManager.AppSettings["SMTPHost"]),
                                 Port = Convert.ToInt32((string)ConfigurationManager.AppSettings["SMTPPort"]),
                                 EnableSsl = Boolean.Parse(ConfigurationManager.AppSettings["EnableSsl"]),
                                 DeliveryMethod = SmtpDeliveryMethod.Network,
                                 Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUsername"], ConfigurationManager.AppSettings["SMTPPassword"])
                             };

            var reader = new ReportReader();
            List<TestReport> reports = reader.GetTestsReports(fileName);
            List<String> emails = TestReport.GetOwnersEmails(reports);

            var emailer = new FailingSmokeTestsEmailer();

            foreach (string email in emails)
            {
                emailer.SendEmail(reports, email, client);
            }
        }
    }
}
