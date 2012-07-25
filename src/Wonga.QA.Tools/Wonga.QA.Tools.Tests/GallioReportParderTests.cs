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
        private string _xmlPath = "";
        
        [SetUp]
        public void SetUp()
        {
            _xmlPath = Path.Combine(Environment.CurrentDirectory, "GallioReport.xml");
        }

        [Test]
        public void CanExtractMetadata()
        {
            var testReport = new GallioReportParser(XDocument.Load(_xmlPath)).GetTestReport();            
        }

        [Test]
        public void CanExtractOwners()
        {
            var testReport = new GallioReportParser(XDocument.Load(_xmlPath)).GetTestReport();
            Assert.IsNotEmpty(testReport.Results.SelectMany(x => x.Children).Where(x => x.Metadata.Any(met => met.Key == "Owner") && x.Metadata.Any(met => met.Key == "OwnerEmail")));
        }

        [Test]
        public void GetTestsWithoutTestFixturesReturnsCorrectResults()
        {
            var testReport = new GallioReportParser(XDocument.Load(_xmlPath)).GetTestReport();
            var testsOnly = testReport.GetTestsWithoutTestFixtures();
        }
    }
}
