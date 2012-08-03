using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca;
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

            var listOfFiles = ScorecardHelper.GetDirectoryFiles();
            _deserializedRequests = listOfFiles.Select(requestFile => ScorecardHelper.DeserializeFromXml<cde_request>(requestFile.XmlTextContent)).ToList();
            
        }

        [Test]
        [Pending]
        public void TestV2LnSeleniumJourney()
        {
            
        }

    }
}
