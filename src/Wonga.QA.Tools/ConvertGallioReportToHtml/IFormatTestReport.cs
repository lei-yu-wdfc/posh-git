using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.ReportConverter
{
    interface IFormatTestReport
    {
        string FormatReport(List<TestResult> testResults);
    }
}
