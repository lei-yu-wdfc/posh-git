using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.Tests
{
    [TestFixture]
    public class GallioReportParderTests
    {
        private string _fullReportXmlPath = "";
        private string _emptyReportXmlPath = "";
        
        [SetUp]
        public void SetUp()
        {
            _fullReportXmlPath = Path.Combine(Environment.CurrentDirectory, "FullGallioReport.xml");
            _emptyReportXmlPath = Path.Combine(Environment.CurrentDirectory, "EmptyGallioReport.xml");
        }

        [Test]
        public void CanExtractMetadata()
        {
            var testReport = new GallioReportParser(XDocument.Load(_fullReportXmlPath)).GetTestReport();
        }

        [Test]
        public void EmptyReportDoesNotCrashTheParser()
        {
            var testReport = new GallioReportParser(XDocument.Load(_emptyReportXmlPath)).GetTestReport();
        }

        [Test]
        public void CanExtractOwners()
        {
            var testReport = new GallioReportParser(XDocument.Load(_fullReportXmlPath)).GetTestReport();
            Assert.IsNotEmpty(testReport.Results.SelectMany(x => x.Children).Where(x => x.Metadata.Any(met => met.Key == "Owner") && x.Metadata.Any(met => met.Key == "OwnerEmail")));
        }

        [Test]
        public void GetTestsWithoutTestFixturesReturnsCorrectResults()
        {
            var testReport = new GallioReportParser(XDocument.Load(_fullReportXmlPath)).GetTestReport();
            var testsOnly = testReport.GetTestsWithoutTestFixtures();
        }
    }
}
