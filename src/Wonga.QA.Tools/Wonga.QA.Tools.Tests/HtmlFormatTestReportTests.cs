using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Tools.ReportConverter;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.Tests
{
    [TestFixture]
    public class HtmlFormatTestReportTests
    {
        [Test]
        public void CanConvertFullReport()
        {
            var testReport =
                new GallioReportParser(XDocument.Load(GallioReportsRepository.GetPathForFullGallioReport())).
                    GetTestReport();
            var converter = new HtmlFormatTestReport().FormatReport(testReport);
        }

        [Test]
        public void CanConvertEmptyReport()
        {
            var testReport =
                new GallioReportParser(XDocument.Load(GallioReportsRepository.GetPathForEmptyGallioReport())).
                    GetTestReport();
            var converter = new HtmlFormatTestReport().FormatReport(testReport);
        }
    }
}
