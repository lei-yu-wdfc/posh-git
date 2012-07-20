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
    class CsvFormatTestReport : IFormatTestReport
    {
        public string FormatReport(List<TestResult> testResults)
        {
            string output = "";
            foreach(var testResult in testResults)
                output += string.Format("{0},{1},{2}\n\r", testResult.Name, testResult.Outcome.ToString(),
                                        testResult.FullName);
            return output;
        }
    }
}
