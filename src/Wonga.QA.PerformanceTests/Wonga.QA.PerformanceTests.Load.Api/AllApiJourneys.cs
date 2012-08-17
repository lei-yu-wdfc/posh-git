using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Api;

namespace Wonga.QA.PerformanceTests.Load.Api
{
    [TestClass]
    public class AllApiJourneys
    {
        [TestInitialize]
        public void InitializeTest()
        {
            //Config.Configure(testTarget: "uk_rc_master");
            //Config.Configure(configsDirectoryPath: "C:\\Users\\francis.chelladurai\\v3QA\\run\\config");
        }

        [TestMethod]
        public void ApiL0JourneyAccepted()
        {
            var journey = new ApiJourneys();
            journey.L0JourneyAccepted();
        }

        [TestMethod]
        public void ApiNoMobilePhoneL0JourneyAccepted()
        {
            var journey = new ApiJourneys();
            journey.ApiNoMobilePhoneL0JourneyAccepted();
        }

        [TestMethod]
        public void ApiL0JourneyDeclined()
        {
            var journey = new ApiJourneys();
            journey.ApiL0JourneyDeclined();
        }

        [TestMethod]
        public void ApiLnJourneyAccepted()
        {
            var journey = new ApiJourneys();
            journey.ApiLnJourneyAccepted();
        }
    }
}
