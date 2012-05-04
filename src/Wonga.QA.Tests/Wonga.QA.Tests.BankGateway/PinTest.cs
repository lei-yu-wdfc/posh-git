using System.Globalization;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.Core;
using System;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, AUT(AUT.Ca)]
    public class PinTest
    {
        private readonly DataDriver _dataDriver;

        public PinTest()
        {
            _dataDriver = new DataDriver();
        }

        [Test, AUT(AUT.Ca), Ignore(), JIRA("CA-2113")]
        public void ShouldGenerateAndWriteThePinDescritorScotia()
        {
            ShouldGenerateAndWriteThePinDescritor("002", "00018");
        }

        //[Test, AUT(AUT.Ca), JIRA("CA-2113"), FeatureSwitch(Constants.BmoFeatureSwitchKey)]
        //public void ShouldGenerateAndWriteThePinDescritorBmo()
        //{
        //    ShouldGenerateAndWriteThePinDescritor("001", "00022");
        //}

        //[Test, AUT(AUT.Ca), JIRA("CA-2113"), FeatureSwitch(Constants.RbcFeatureSwitchKey)]
        //public void ShouldGenerateAndWriteThePinDescritorRbc()
        //{
        //    ShouldGenerateAndWriteThePinDescritor("003", "00005");
        //}
        
        private void ShouldGenerateAndWriteThePinDescritor(string institutionNumber, string branchNumber)
        {
            var bankGatewayOutgoingFile = new BankGatewayOutgoingFileReader();

            var customer = CustomerBuilder.New().
                WithInstitutionNumber(institutionNumber).
                WithBranchNumber(branchNumber).
                Build();

            var application = ApplicationBuilder.New(customer).Build();

            string bankAccountNumber = customer.BankAccountNumber.ToString(CultureInfo.InvariantCulture);

            var bankAccountBase = Do.Until(() => _dataDriver.Payments.Db.BankAccountsBase.FindByAccountNumber(bankAccountNumber));
            var personalBankAccountsCa = Do.Until(() => _dataDriver.Payments.Db.PersonalBankAccountsCa.FindByBankAccountId(bankAccountBase.BankAccountId));

            application.RepayOnDueDate();

            Assert.IsTrue(bankGatewayOutgoingFile.VerifyPinContainedInBankGatewayFileSent(bankAccountNumber, personalBankAccountsCa.Pin));
        }
    }
}
