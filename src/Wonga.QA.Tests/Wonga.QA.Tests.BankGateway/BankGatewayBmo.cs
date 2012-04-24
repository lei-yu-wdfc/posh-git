﻿using System;
using System.Collections.Generic;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.BankGateway;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.OpsSagasCa;
using Wonga.QA.Framework.Mocks;
using Wonga.QA.Framework.Msmq;
using Wonga.QA.Tests.BankGateway.Helpers;
using Wonga.QA.Tests.Core;
using ProvinceEnum = Wonga.QA.Framework.Api.ProvinceEnum;

namespace Wonga.QA.Tests.BankGateway
{
    [TestFixture, FeatureSwitch(Constants.BmoFeatureSwitchKey)]
    public class BankGatewayBmo
    {
        private const string BankGatewayIsTestModeKey = "BankGateway.IsTestMode";
        private string _bankGatewayIsTestMode;
        private const string MocksBmoEnabledKey = "Mocks.BmoEnabled";
        private string _mocksBmoEnabled;
        private static readonly Random Random = new Random(Environment.TickCount);

        [SetUp]
        public void SetUp()
        {
            ServiceConfigurationEntity bankGatewayIsTestModeKeyEntity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == BankGatewayIsTestModeKey);
            _bankGatewayIsTestMode = bankGatewayIsTestModeKeyEntity.Value;
            bankGatewayIsTestModeKeyEntity.Value = "false";
            bankGatewayIsTestModeKeyEntity.Submit();

            ServiceConfigurationEntity mocksScotiaEnabledKeyEntity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == MocksBmoEnabledKey);
            _mocksBmoEnabled = mocksScotiaEnabledKeyEntity.Value;
            mocksScotiaEnabledKeyEntity.Value = "true";
            mocksScotiaEnabledKeyEntity.Submit();
        }

        [TearDown]
        public void TearDown()
        {
            ServiceConfigurationEntity bankGatewayIsTestModeKeyEntity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == BankGatewayIsTestModeKey);
            bankGatewayIsTestModeKeyEntity.Value = _bankGatewayIsTestMode;
            bankGatewayIsTestModeKeyEntity.Submit();

            ServiceConfigurationEntity mocksScotiaEnabledKeyEntity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == MocksBmoEnabledKey);
            mocksScotiaEnabledKeyEntity.Value = _mocksBmoEnabled;
            mocksScotiaEnabledKeyEntity.Submit();
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1914")]
        public void WhenLoanGoesToBmoThenItShouldRecieveSettlementReport200ToIndicateASuccessfulCashOut()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;

            var customer = CustomerBuilder.New().
                                WithInstitutionNumber("001").
                                WithBranchNumber("00022").
                                ForProvince(ProvinceEnum.ON).
                                Build();

            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            TransactionEntity transaction = WaitBankGatewayFunctions.WaitForStatusOfTransaction(application.Id);

            WaitBankGatewayFunctions.WaitForAckForTransaction(transaction);
        }
    }
}
