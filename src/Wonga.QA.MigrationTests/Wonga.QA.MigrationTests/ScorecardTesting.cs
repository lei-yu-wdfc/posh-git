using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.MigrationTests.Utils;
using System.Collections;

namespace Wonga.QA.MigrationTests
{
    class ScorecardTesting
    {
        private IEnumerable<cde_request> _deserializedRequests;

        [SetUp]
        public void SetUp()
        {
            ScorecardHelper.GetDirectoryFiles();
            //var listOfFiles = ScorecardHelper.GetDirectoryFiles();
            //_deserializedRequests = listOfFiles.Select(requestFile => ScorecardHelper.DeserializeFromXml<cde_request>(requestFile.XmlTextContent)).ToList();
            //var xxx = ScorecardHelper.RunV3LnJourneyFromV2LnCdeRequest((_deserializedRequests.Single()));
        }

        [Test]
        [Pending]
        [Description("This is the V3 Scorecard Testing. For any info please contact alex.pricope@wonga.com")]
        public void TestV2LnSeleniumJourney()
        {
            //Note:1) I have a list of V2 deserialized requests

            //Note:2) For each of this, run V3


        }

        [TearDown]
        public void Shutdown()
        {
            
        }
    }
}
