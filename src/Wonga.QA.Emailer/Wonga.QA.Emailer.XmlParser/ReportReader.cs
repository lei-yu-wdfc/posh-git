using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Wonga.QA.Emailer.Domain;

namespace Wonga.QA.Emailer.XmlParser
{
    public class ReportReader
    {

        public List<TestReport> GetTestsReports(string fileName)
        {
            var reports = new List<TestReport>();
            var Tests = new List<XElement>();
            XElement doc;
            try
            {
                doc = XElement.Load(fileName); // if file not xml - do nothing
            }
            catch (Exception)
            {
                throw new Exception("Can't read file" + fileName);
            }

            if (MoreThenTwentyPersentFailes(fileName)) //if in report more then 20% of tests are failed - do nothing
                return reports;

            var sut = fileName.Remove(0, fileName.IndexOf("_"));
            sut = sut.Remove(sut.LastIndexOf("_"));
            sut = sut.Replace("_", "");
            var aut = fileName.Remove(0, fileName.LastIndexOf("_"));
            aut = aut.Remove(aut.LastIndexOf("."));
            aut = aut.Replace("_", "");
            TestReport.SUT = sut;
            TestReport.AUT = aut;

            IEnumerable<XElement> result = (from c in doc.Element("{http://www.gallio.org/}testPackageRun")
                                        .Element("{http://www.gallio.org/}testStepRun")
                                        .Element("{http://www.gallio.org/}children")
                                        .Element("{http://www.gallio.org/}testStepRun")
                                        .Element("{http://www.gallio.org/}children")
                                        .Elements("{http://www.gallio.org/}testStepRun")
                                            select c);//Get results for files

            foreach (var xElement in result)
            {
                IEnumerable<XElement> tests = (from c in xElement.Element("{http://www.gallio.org/}children")
                                                      .Elements("{http://www.gallio.org/}testStepRun")
                                               select c);//Select failed tests
                foreach (XElement test in tests)
                {
                    Tests.Add(test);

                }
            }

            foreach (XElement test in Tests)
            {
                var report = new TestReport()
                                 {
                                     Failed = test.Element("{http://www.gallio.org/}result")
                                                      .Element("{http://www.gallio.org/}outcome")
                                                      .Attribute("status").Value.Equals("failed"),
                                     TestName = test.Element("{http://www.gallio.org/}testStep").Attribute("name").Value,
                                     FullTestName = test.Element("{http://www.gallio.org/}testStep").Attribute("fullName").Value,
                                     JIRA = (from c in test.Element("{http://www.gallio.org/}testStep")
                                                 .Element("{http://www.gallio.org/}metadata")
                                                 .Elements("{http://www.gallio.org/}entry")
                                             where c.Attribute("key").Value.Equals("JIRA")
                                             select c.Element("{http://www.gallio.org/}value").Value).FirstOrDefault(),
                                     StartTime = test.Attribute("startTime").Value
                                 }; //Get metadata of test



                #region Get multi Emails
                XElement entryOwnerEmails = (from c in test.Element("{http://www.gallio.org/}testStep")
                                       .Element("{http://www.gallio.org/}metadata")
                                       .Elements("{http://www.gallio.org/}entry")
                                             where c.Attribute("key").Value.Equals("OwnerEmail")
                                             select c).SingleOrDefault();

                if (entryOwnerEmails != null)
                {
                    IEnumerable<XElement> ownerEmails = (from c in entryOwnerEmails.Elements("{http://www.gallio.org/}value") select c);
                    foreach (XElement ownerEmail in ownerEmails)
                    {
                        report.OwnerEmails.Add(ownerEmail.Value);
                    }
                }
                #endregion
                var reader = test.CreateReader();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.CDATA:
                            report.Message += reader.Value; //Get error message
                            break;
                    }
                }
                reports.Add(report);
            }
            return reports;
        }
        private bool MoreThenTwentyPersentFailes(string fileName)
        {
            var doc = XElement.Load(fileName);
            int runCount = Int32.Parse(doc.Element("{http://www.gallio.org/}testPackageRun").Element("{http://www.gallio.org/}statistics").
                    Attribute("runCount").Value);
            int failedCount = Int32.Parse(doc.Element("{http://www.gallio.org/}testPackageRun").Element("{http://www.gallio.org/}statistics").
                    Attribute("failedCount").Value);
            if (failedCount >= runCount * 0.2)
            {
                return true;
            }
            return false;
        }
    }
}
