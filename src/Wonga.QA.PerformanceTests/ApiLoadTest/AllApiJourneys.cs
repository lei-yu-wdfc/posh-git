using MbUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace ApiLoadTest
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
        public void L0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
        }

        [TestMethod]
        public void ApiNoMobilePhoneL0JourneyAccepted()
        {
            Customer cust = CustomerBuilder.New().WithMobileNumber(null).Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).WithLoanAmount(200).Build();
        }

        [TestMethod]
        public void ApiL0JourneyDeclined()
        {
            Customer cust = CustomerBuilder.New().WithEmployer("Wonga").Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }

        [TestMethod]
        public void ApiNoMobilePhoneL0JourneyDeclined()
        {
            Customer cust = CustomerBuilder.New().WithEmployer("Wonga").WithMobileNumber(null).Build();
            ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Declined).Build();
        }
    }
}
