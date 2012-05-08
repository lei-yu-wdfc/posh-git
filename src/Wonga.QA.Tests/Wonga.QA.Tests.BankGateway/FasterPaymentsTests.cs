using System.Globalization;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Tests.BankGateway.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, AUT(AUT.Ca)]
    public class FasterPaymentsTests
    {
        private readonly DataDriver _dataDriver;

        private Customer _customer;
        private string _bankAccountNumber;
        private string _pin;

        public FasterPaymentsTests()
        {
            _dataDriver = new DataDriver();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2113")]
        public void ShouldGenerateAndWriteThePinDescritorScotia()
        {
            ShouldGenerateAndWriteThePinDescritor("002", "00018");
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2113")]
        public void ShouldUseThePinDescritorOnNextCashinScotia()
        {
            ShouldGenerateAndWriteThePinDescritor("002", "00018");
            ShouldUseThePinDescritorOnNextCashin();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-2113"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey)]
        public void ShouldGenerateAndWriteThePinDescritorBmo()
        {
            ShouldGenerateAndWriteThePinDescritor("001", "00022");
        }
        
        private void ShouldGenerateAndWriteThePinDescritor(string institutionNumber, string branchNumber)
        {
            _customer = CustomerBuilder.New().
                                WithInstitutionNumber(institutionNumber).
                                WithBranchNumber(branchNumber).
                                Build();

            var application = ApplicationBuilder.New(_customer).Build();

            _bankAccountNumber = _customer.BankAccountNumber.ToString(CultureInfo.InvariantCulture);

            int bankAccountId = Do.Until(() => _dataDriver.Payments.Db.BankAccountsBase.FindByAccountNumber(_bankAccountNumber)).BankAccountId;
            _pin = Do.Until(() => _dataDriver.Payments.Db.PersonalBankAccountsCa.FindByBankAccountId(bankAccountId)).Pin;

            application.RepayOnDueDate();

            var bankGatewayOutgoingFile = new BankGatewayOutgoingFileReader();
            Assert.IsTrue(bankGatewayOutgoingFile.VerifyPinContainedInBankGatewayFileSent(_bankAccountNumber, _pin));
        }

        private void ShouldUseThePinDescritorOnNextCashin()
        {
            var application = ApplicationBuilder.New(_customer).Build();
            
            application.RepayOnDueDate();

            var bankGatewayOutgoingFile = new BankGatewayOutgoingFileReader();
            Assert.IsTrue(bankGatewayOutgoingFile.VerifyPinContainedInBankGatewayFileSent(_bankAccountNumber, _pin));
        }
    }
}
