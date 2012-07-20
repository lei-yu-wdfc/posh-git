using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConvertGallioReportToHtml
{
    interface IFormatTestReport
    {
        string FormatReport(List<TestResult> testResults);
    }
}
