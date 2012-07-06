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
using System.Threading;
using System.Timers;
using Wonga.QA.Emailer.Domain;
using Wonga.QA.Emailer.Plugins.SendFailingSmokeTestEmails;
using Wonga.QA.Emailer.Plugins.SendWarningEmail;
using Wonga.QA.Emailer.XmlParser;
using Timer = System.Timers.Timer;

namespace Wonga.QA.Emailer.Service
{
    public partial class Service : ServiceBase
    {
        private Timer _timer;

        SmtpClient client = new SmtpClient()
        {
            Host = (ConfigurationManager.AppSettings["SMTPHost"]),
            Port = Convert.ToInt32((string)ConfigurationManager.AppSettings["SMTPPort"]),
            EnableSsl = Boolean.Parse(ConfigurationManager.AppSettings["EnableSsl"]),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUsername"], ConfigurationManager.AppSettings["SMTPPassword"])
        };

        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AddLog("Wonga.QA.Emailer Service start!");
            CreateTimer();
        }

        protected override void OnStop()
        {
            AddLog("Wonga.QA.Emailer Service stop.");
            _timer.Enabled = false;
        }

        protected override void OnContinue()
        {
            AddLog("Wonga.QA.Emailer Service continue.");
        }

        protected override void OnPause()
        {
            AddLog("Wonga.QA.Emailer Service paused.");
        }

        public void AddLog(string log)
        {
            if (!EventLog.SourceExists("Wonga.QA.Emailer"))
            {
                EventLog.CreateEventSource("Wonga.QA.Emailer", "Wonga.QA.Emailer");
            }
            eventLog.Source = "Wonga.QA.Emailer";
            eventLog.WriteEntry(log);
        }

        private void CreateTimer()
        {
            _timer = new Timer { Enabled = true, Interval = 300000, AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var folderPath = ConfigurationManager.AppSettings["TempFolderPath"];
            var reports = Directory.GetFiles(folderPath);
            var warningEmailer = new WarningEmailer();
            if (reports.Count() != 0)
            {
                foreach (var report in reports)
                {
                    try
                    {
                        ParseReportAndSendEmails(report);
                        AddLog("file " + report + " was processed.");
                        File.Move(report, ConfigurationManager.AppSettings["DoneFolderPath"] + report.Remove(0, report.LastIndexOf(@"\")));
                        AddLog("file " + report.Remove(0, report.LastIndexOf(@"\")) + " was moved to Done folder.");
                    }
                    catch (Exception exception)
                    {
                        warningEmailer.SendWarning(exception.Message, ConfigurationManager.AppSettings["SMTPUsername"], client);
                        File.Move(report, ConfigurationManager.AppSettings["ErrorFolderPath"] + report.Remove(0, report.LastIndexOf(@"\")));
                        AddLog("file " + report.Remove(0, report.LastIndexOf(@"\")) + " was moved to Error folder.");
                    }

                }
            }
            else
            {
                AddLog("Temp folder is empty.");
            }
        }

        private void ParseReportAndSendEmails(string fileName)
        {
            var reader = new ReportReader();
            var emailer = new FailingTestsEmailer();
            List<TestReport> reports;
            List<String> emails;

            reports = reader.GetTestsReports(fileName);
            emails = TestReport.GetOwnersEmails(reports);
            foreach (string email in emails)
            {
                emailer.SendEmail(reports, email, client);
            }

        }
    }
}
