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
        private XDocument _fullReportDoc;
        private XDocument _emptyReportDoc;

        [SetUp]
        public void SetUp()
        {
            _fullReportDoc = XDocument.Load(GallioReportsRepository.GetPathForFullGallioReport());
            _emptyReportDoc = XDocument.Load(GallioReportsRepository.GetPathForEmptyGallioReport());
        }

        [Test]
        public void CanExtractMetadata()
        {
            var testReport = new GallioReportParser(_fullReportDoc).GetTestReport();
        }

        [Test]
        public void EmptyReportDoesNotCrashTheParser()
        {
            var testReport = new GallioReportParser(_emptyReportDoc).GetTestReport();
        }

        [Test]
        public void CanExtractOwners()
        {
            var testReport = new GallioReportParser(_fullReportDoc).GetTestReport();
            Assert.IsNotEmpty(testReport.Results.SelectMany(x => x.Children).Where(x => x.Metadata.Any(met => met.Key == "Owner") && x.Metadata.Any(met => met.Key == "OwnerEmail")));
        }

        [Test]
        public void GetTestsWithoutTestFixturesReturnsCorrectResults()
        {
            var testReport = new GallioReportParser(_fullReportDoc).GetTestReport();
            var testsOnly = testReport.GetTestsWithoutTestFixtures();
        }

        [Test]
        public void CanStoreMultipleMetadataOfTheSameKey()
        {
            var testReport = new GallioReportParser(_fullReportDoc).GetTestReport();
            testReport.GetTestsWithoutTestFixtures().Any(x => x.Metadata.ContainsKey("Aut") && x.Metadata["Aut"].Count > 1);
        }
    }
}
