using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.ReportConverter
{
    public class CsvFormatTestReport : IFormatTestReport
    {
        public string FormatReport(TestReport testReport)
        {
            string output = "";
            foreach (var test in testReport.GetTestsWithoutTestFixtures())
                    output += string.Format("{0},{1},{2}\n\r", test.Name, test.Outcome.ToString(),
                                        test.FullName);
            return output;
        }
    }
}
