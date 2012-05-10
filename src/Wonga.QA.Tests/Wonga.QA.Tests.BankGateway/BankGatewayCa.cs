using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Tests.BankGateway.Enums;
using Wonga.QA.Tests.BankGateway.Helpers;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, AUT(AUT.Ca), Parallelizable(TestScope.All)]
    public class BankGatewayCa
    {
        private readonly dynamic _bgTrans = Drive.Data.BankGateway.Db.Transactions;

        [Test, AUT(AUT.Ca), JIRA("CA-1913")]
        public void WhenCustomerEntersAnApplicationWithAValidBankAccountThenBankGatewayShouldUpdateTransactionAsPaid()
        {
            var customer = CustomerBuilder.New().
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                        _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid).Single());

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913")]
        public void WhenCustomerEntersAnApplicationWithAnInvalidBankAccountThenBankGatewayShouldUpdateTransactionAsFailed()
        {
            var customer = CustomerBuilder.New().
                                Build();

            ScotiaResponseBuilder.New().
                ForBankAccountNumber(customer.BankAccountNumber).
                Reject();

            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                        _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Failed).Single());
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey, true)]
        public void WhenCustomerEntersAnApplicationWithInstitionCode001ThenBankGatewayShouldRouteTransactionToScotia()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid)).Single();

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey, true)]
        public void WhenCustomerEntersAnApplicationWithInstitionCode003ThenBankGatewayShouldRouteTransactionToScotia()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("003").
                                WithBranchNumber("00022").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.TransactionStatus == (int)BankGatewayTransactionStatus.Paid)).Single();

        }

        [Test, AUT(AUT.Ca), JIRA("CA-1913"), FeatureSwitch(FeatureSwitchConstants.BmoFeatureSwitchKey)]
        public void WhenCustomerEntersInstitionNumber002ThenBankGatewayShouldRouteTransactionToScotia()
        {
            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("002").
                                WithBranchNumber("00018").
                                Build();
            var application = ApplicationBuilder.New(customer).Build();

            Do.Until(() => _bgTrans.FindAll(_bgTrans.ApplicationId == application.Id &&
                                    _bgTrans.BankIntegrationId == (int)BankGatewayIntegrationId.Scotia).Single());

        }
    }
}