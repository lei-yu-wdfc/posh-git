using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Emailer.Domain
{
    public class TestReport
    {
        public static String SUT { get; set; }
        public static String AUT { get; set; }
        public bool Failed { get; set; }
        public String TestName { get; set; }
        public String FullTestName { get; set; }
        private List<String> _ownerEmails = new List<string>();
        public List<String> OwnerEmails
        {
            get { return _ownerEmails; }
            set { _ownerEmails = value; }
        }
        public String JIRA { get; set; }
        public String Message { get; set; }
        public String StartTime { get; set; }

        public static List<String> GetOwnersEmails(List<TestReport> reports)
        {
            var emails = new List<string>();
            foreach (TestReport report in reports)
            {
                if (report.Failed) //get autor email only for failed tests
                {
                    foreach (string autor in report.OwnerEmails)
                    {
                        if (!emails.Contains(autor))
                        {
                            emails.Add(autor);
                        }
                    }
                }
            }
            return emails;
        }
    }
}
