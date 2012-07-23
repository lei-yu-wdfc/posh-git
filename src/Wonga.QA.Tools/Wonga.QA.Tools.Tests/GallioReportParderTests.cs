using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Tools.Library;

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
            var results = new GallioReportParser(XDocument.Load(_xmlPath)).Parse();
        }

        [Test]
        public void CanExtractOwners()
        {
            var results = new GallioReportParser(XDocument.Load(_xmlPath)).Parse();
            Assert.IsNotEmpty(results.Where(x => x.Metadata.Any(met => met.Key == "Owner") && x.Metadata.Any(met => met.Key == "OwnerEmail")));

        }
    }
}
