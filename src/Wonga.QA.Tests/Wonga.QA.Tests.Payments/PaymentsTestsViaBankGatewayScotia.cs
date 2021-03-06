﻿using System;
using System.Linq; 
using System.Collections.Generic;
using Gallio.Framework;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Mocks.Entities;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Payments.Helpers;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Payments
{
    [Parallelizable(TestScope.Self)]
    public class PaymentsTestsViaBankGatewayScotia
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Parallelizable]
        public void VerifyOnlineBillPaymentCreditedToCustomerAccount()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int dayOfLoanTermToRepay = 5;
            const int earlyRepaymentAmount = 104;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

            Drive.Db.RewindToDayOfLoanTerm(application.Id, dayOfLoanTermToRepay);

            var transaction = new OnlineBillPaymentTransaction
                                  {
                                      AmountInCent = earlyRepaymentAmount*100,
                                      Ccin = customer.GetCcin(),
                                      CustomerFullName = customer.GetCustomerFullName(),
                                      ItemNumber = 1,
                                      RemittancePaymentDate = DateTime.UtcNow,
                                      RemittanceTraceNumber =
                                          Get.RandomInt(100000000, 999999999).ToString()
                                  };

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(application.Id.ToString(), new List<OnlineBillPaymentTransaction> { transaction });

            var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, dayOfLoanTermToRepay-1);

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((loanAmount + expectedInterestAmountApplied) * -1));

            var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
            Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Parallelizable]
        public void VerifyLoanClosesAfterFullOnlineBillPaymentRecieved()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int dayOfLoanTermToRepay = 5;
            const int earlyRepaymentAmount = 104;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();

            Drive.Db.RewindToDayOfLoanTerm(application.Id, dayOfLoanTermToRepay);

            var transaction = new OnlineBillPaymentTransaction
                                  {
                                      AmountInCent = earlyRepaymentAmount*100,
                                      Ccin = customer.GetCcin(),
                                      CustomerFullName = customer.GetCustomerFullName(),
                                      ItemNumber = 1,
                                      RemittancePaymentDate = DateTime.UtcNow,
                                      RemittanceTraceNumber =
                                          Get.RandomInt(100000000, 999999999).ToString()
                                  };

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(application.Id.ToString(), new List<OnlineBillPaymentTransaction> { transaction });

            Do.With.Timeout(1).Until(() => application.IsClosed);
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyPartialOnlineBillPaymentDoesNotCloseLoan()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int dayOfLoanTermToRepay = 5;
            const int earlyRepaymentAmount = 50;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

			Drive.Db.RewindToDayOfLoanTerm(application.Id, dayOfLoanTermToRepay);

            var transaction = new OnlineBillPaymentTransaction
                                  {
                                      AmountInCent = earlyRepaymentAmount*100,
                                      Ccin = customer.GetCcin(),
                                      CustomerFullName = customer.GetCustomerFullName(),
                                      ItemNumber = 1,
                                      RemittancePaymentDate = DateTime.UtcNow,
                                      RemittanceTraceNumber =
                                          Get.RandomInt(100000000, 999999999).ToString()
                                  };

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(application.Id.ToString(), new List<OnlineBillPaymentTransaction> { transaction });
            var directBankPaytransaction = WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, (earlyRepaymentAmount * -1));

            TimeoutPaymentFunctions.TimeoutCloseApplicationSaga(directBankPaytransaction);

            //TODO: Verify application does not close... 
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentDateRealignment()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int dayOfLoanTermToRepay = 5;
            const int earlyRepaymentAmount = 102;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

			Drive.Db.RewindToDayOfLoanTerm(application.Id, dayOfLoanTermToRepay);

            //create online bill payment file for customer.. date = loanCreated - 5 days + 2 extra days... i.e file not processed till 7 days into loan
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file

            OnlineBillPaymentTransaction transaction = new OnlineBillPaymentTransaction
            {
                AmountInCent = earlyRepaymentAmount * 100,
                Ccin = customer.GetCcin(),
                CustomerFullName = customer.GetCustomerFullName(),
                ItemNumber = 1,
                RemittancePaymentDate = DateTime.UtcNow.Subtract(new TimeSpan(2, 0, 0, 0)),
                RemittanceTraceNumber = Get.RandomInt(100000000, 999999999).ToString()
            };

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(application.Id.ToString(), new List<OnlineBillPaymentTransaction> { transaction });

            //var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, dayOfLoanTermToRepay - 1);

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, (earlyRepaymentAmount * -1));

            //var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
            //Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentRecodredToDbForUnrecognisedCustomer()
        {
            //create online bill payment file for a non existent customer.. 
            //trigger mock to intitiate bank gateway to process file
            //verify file recorded db

            String ccin = Get.RandomInt(100000000, 999999999).ToString();
            TestLog.DebugTrace.WriteLine("ccin -> {0}\n", ccin);

            var transaction = new OnlineBillPaymentTransaction
            {
                AmountInCent = 10000,
                Ccin = ccin,
                CustomerFullName = Get.GetName()+" "+Get.GetName(),
                ItemNumber = 1,
                RemittancePaymentDate = DateTime.UtcNow,
                RemittanceTraceNumber = Get.RandomInt(100000000, 999999999).ToString()
            };

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(Guid.NewGuid().ToString(), new List<OnlineBillPaymentTransaction> { transaction });

            Assert.IsTrue(VerifyPaymentFunctions.VerifyOnlineBillPaymentRecordForCcin(ccin));

            //Todo: Have some way to verify an event was created (as the customer is not recognised) and an email was generated as a result of the event. The email will be sent to finance
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentOverRepaymentIsRecordedToDb()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            const int dayOfLoanTermToRepay = 5;
            const int earlyRepaymentAmount = 104;
            const int overpayAmount = 10;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

			Drive.Db.RewindToDayOfLoanTerm(application.Id, dayOfLoanTermToRepay);

            //create online bill payment file for customer.. amount should be greater than balance....
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file

            OnlineBillPaymentTransaction transaction = new OnlineBillPaymentTransaction
            {
                AmountInCent = (earlyRepaymentAmount + overpayAmount) * 100,
                Ccin = customer.GetCcin(),
                CustomerFullName = customer.GetCustomerFullName(),
                ItemNumber = 1,
                RemittancePaymentDate = DateTime.UtcNow,
                RemittanceTraceNumber = Get.RandomInt(100000000, 999999999).ToString()
            };

            TestLog.DebugTrace.WriteLine("ccin -> {0}\n", customer.GetCcin());

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(application.Id.ToString(), new List<OnlineBillPaymentTransaction> { transaction });

            //var expectedInterestAmountApplied = CalculateFunctionsCa.CalculateExpectedVariableInterestAmountAppliedCa(loanAmount, dayOfLoanTermToRepay - 1);

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((earlyRepaymentAmount + overpayAmount) * -1));

            //var actualInterestAmountApplied = GetPaymentFunctions.GetInterestAmountApplied(applicationId);
            //Assert.IsTrue(VerifyPaymentFunctions.VerifyVariableInterestCharged(actualInterestAmountApplied, expectedInterestAmountApplied));

            //verify recorded in db that customer has overpaid... 
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentForCustomerInArrears()
        {
            const int loanTerm = 15;
            const decimal loanAmount = 100;
            //const int dayOfLoanTermToRepay = 5;
            const int earlyRepaymentAmount = 104;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

            application.PutIntoArrears(3);

            //create online bill payment file for customer.. date = loanCreated - 5 days...
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file

            OnlineBillPaymentTransaction transaction = new OnlineBillPaymentTransaction
            {
                AmountInCent = earlyRepaymentAmount * 100,
                Ccin = customer.GetCcin(),
                CustomerFullName = customer.GetCustomerFullName(),
                ItemNumber = 1,
                RemittancePaymentDate = DateTime.UtcNow,
                RemittanceTraceNumber = Get.RandomInt(100000000, 999999999).ToString()
            };

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(application.Id.ToString(), new List<OnlineBillPaymentTransaction> { transaction });

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((earlyRepaymentAmount) * -1));
        }

        [Test, AUT(AUT.Ca), JIRA("CA-1441"), Ignore("Not fully implemented, do not run")]
        public void VerifyOnlineBillPaymentForCustomerInArrearsFullRepayment()
        {
            const int loanTerm = 10;
            const decimal loanAmount = 100;
            const int daysInArrears = 5;
            const decimal repaymentAmount = 130.45m;

            var customer = CustomerBuilder.New().ForProvince(ProvinceEnum.ON).Build();
            var application = ApplicationBuilder.New(customer).WithLoanTerm(loanTerm).WithLoanAmount(loanAmount).Build();
            var applicationId = application.Id;

            application.PutIntoArrears(daysInArrears);

            //create online bill payment file for customer.. date = loanCreated - 5 days...
            //insert file data to database table
            //trigger mock to intitiate bank gateway to process file

            OnlineBillPaymentTransaction transaction = new OnlineBillPaymentTransaction
            {
                AmountInCent = (int)(repaymentAmount * 100),
                Ccin = customer.GetCcin(),
                CustomerFullName = customer.GetCustomerFullName(),
                ItemNumber = 1,
                RemittancePaymentDate = DateTime.UtcNow,
                RemittanceTraceNumber = Get.RandomInt(100000000, 999999999).ToString()
            };

            Drive.Mocks.Scotia.AddOnlineBillPaymentFile(application.Id.ToString(), new List<OnlineBillPaymentTransaction> { transaction });

            WaitPaymentFunctions.WaitForTransactionTypeOfDirectBankPayment(applicationId, ((repaymentAmount) * -1));
        }

    }
}
