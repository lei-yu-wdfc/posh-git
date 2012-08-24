
using System;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.DataTests.Hds.Common;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Data;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands;
using Wonga.QA.Framework.Old;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.DataTests.BiCustomerManagement
{
    /// <summary>
    /// This class is intended to test the flow of data from BiCustomerManagement.ApplicationStatusHistory to DIWarehouse.ApplicationStatusHistory
    /// </summary>
    [TestFixture]
    [Category("AcceptanceTest")]
    [Category("ApplicationStatusHistory")]
    [JIRA("DI-593"), Owner(Owner.JimmyJose)]
    class ApplicationStatusHistoryTests
    {
        //Tables to check
        private static string _tablename = "ApplicationStatusHistory";

        //Connection to different databases were the data is flowing from sevices to datawarehouse
        dynamic _serviceApplicationStatusHistory = Drive.Data.BiCustomerManagement.Db.bicustomermanagement[_tablename];
        dynamic _warehouseApplicationStatusHistory = Drive.Data.Warehouse.Db.dw[_tablename];

        //SQL Server Agent jobs
        private HdsUtilitiesAgentJob _sqlserverAgentJobs = null;
        private bool _cdcStagingAgentJobWasStopped;
        private bool _hdsAgentJobWasStopped;


        [FixtureSetUp]
        [Description("This is the text fixture setup for all tests")]
        public void FixtureSetup()
        {
            _sqlserverAgentJobs = new HdsUtilitiesAgentJob(HdsUtilitiesBase.WongaService.BiCustomerManagement);
            _cdcStagingAgentJobWasStopped = _sqlserverAgentJobs.StartJob(_sqlserverAgentJobs.CdcStagingAgentJob);
            _hdsAgentJobWasStopped = _sqlserverAgentJobs.StartJob(_sqlserverAgentJobs.HdsLoadAgentJob);

        }

        [FixtureTearDown]
        [Description("This is the text fixture teardown for all tests")]
        public void FixtureTearDown()
        {
            if (_cdcStagingAgentJobWasStopped)
            {
                _sqlserverAgentJobs.StopJob(_sqlserverAgentJobs.CdcStagingAgentJob);
            }

            if (_hdsAgentJobWasStopped)
            {
                _sqlserverAgentJobs.StopJob(_sqlserverAgentJobs.HdsLoadAgentJob);
            }

        }

        /// <summary>
        /// Verifies when an application status is 'Live' the status history will have entries for the pre-live events (Accepted, TermsAgreed, Live).
        /// 3 rows are created with 3 different status when an application is created.
        /// </summary>
        [Test]
        [Description("Verifies that when an application status is 'Live' the status history will have entries for the pre-live events (New, Accepted, TermsAgreed, Live)")]
        public void L0JourneyAcceptedApplicationStatusHistory()
        {
            //Arrange
            Customer cust = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            //Act & Assert
            CheckDiWarehouseApplicationStatusHistory(application, false);
        }

        /// <summary>
        /// Verifies when an application status is 'PaidInFull' the status history will have entries for the pre-live events (Accepted, TermsAgreed, Live, Due Today, PaidInFull).
        /// 5 rows are created when a L0 application is closed in the normal workflow.
        /// </summary>
        [Test]
        [Description("Verifies that when an application status is 'PaidInFull' the status history will have entries for the pre-live events (Accepted, TermsAgreed, Live, Due Today, PaidInFull)")]
        public void RepayingOnDueDateClosesApplicationApplicationStatusHistory()
        {
            //Arrange
            Customer cust = CustomerBuilder.New().Build();
            Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            //Act & Assert
            CheckDiWarehouseApplicationStatusHistory(application, false);
            //Act
            application.RepayOnDueDate();
            //Assert
            CheckDiWarehouseApplicationStatusHistory(application, false);
        }

        /// <summary>
        /// Verifies application status hisory for Ln application with second application going into Arreas, Hardship and WritenOff stages.
        /// Verifies when an application status is 'Written Off' and have the following status (Accepted, TermsAgreed, Live, InArrears, hardship, WrittenOff).
        /// 11 rows are created while running the test in service DB. These rows are moved into the warehouse and the application is updated with latest status.
        /// </summary>
        [Test]
        public void LnJourneyAcceptedApplicationStatusHistory()
        {
            Customer cust = CustomerBuilder.New().Build();
            //Create the first application for the customer
            Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();
            application.RepayOnDueDate();
            //Create the second Application for the same customer
            Application application1 = ApplicationBuilder.New(cust).Build();
            //Load the Datawarehouse with 8 records which contains 2 application (PaidInFull and Live)
            CheckDiWarehouseApplicationStatusHistory(application, false);
            //Put the application into Arrears
            application1.PutIntoArrears(50);
            //Load the Datawarehouse with 1 records for the second application (InArrears) and updates the [dw].[Application].[ApplicationStatusSK] with the new status(InArrears).
            CheckDiWarehouseApplicationStatusHistory(application1, false);

            // Put the application into Hardship
            var cmd = new CsReportHardshipCommand()
            {
                AccountId = application1.AccountId,
                ApplicationId = application1.Id,
                CaseId = Guid.NewGuid()
            };
            Drive.Cs.Commands.Post(cmd);
            //Load the Datawarehouse with 1 records for the second application (Hardship) and updates the [dw].[Application].[ApplicationStatusSK] with the new status(Hardship).
            CheckDiWarehouseApplicationStatusHistory(application1, false);

            //write off the Application
            var cmd1 = new WriteOffApplicationCommand()
            {
                ApplicationId = application1.Id,
                DoNotRelend = true
            };
            Drive.Cs.Commands.Post(cmd1);
            //Load the Datawarehouse with 1 records for the second application (WrittenOff) and updates the [dw].[Application].[ApplicationStatusSK] with the new status(WrittenOff).
            CheckDiWarehouseApplicationStatusHistory(application1, false);
        }



        private void CheckDiWarehouseApplicationStatusHistory(Application application, bool jobSuccess)
        {
            // Checking validy of data from service DB to HDS DB.

            // Allow for CDC to pick up the data
            Thread.Sleep(20000);
            //Arrange
            var sourceData = Do.Until(() => _serviceApplicationStatusHistory.FindAllBy(ApplicationId: application.Id));
            int lastItem = sourceData.Count() - 1;
            int _count = 0;
            //var last = sourceData.OrderByIdDescending().First();
            // Allow for CDC to pick up the data
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);
            _sqlserverAgentJobs.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.CDCStaging);
            Thread.Sleep(HdsUtilitiesAgentJob.CdcWaitTimeMilliseconds);
            _sqlserverAgentJobs.WaitForLoadExecutionCycle(HdsUtilitiesBase.SystemComponent.HDS);
            Thread.Sleep(20000);

            foreach (var sourceDataRow in sourceData)
            {
                //Checking validy of data from service DB to DIWAREHOUSE DB.
                //Act
                if (jobSuccess == false)
                {
                    jobSuccess = SQLServerAgentJobs.Execute(_sqlserverAgentJobs.DiWarehouseWongaHdsLoadAgentJob);
                    Thread.Sleep(30000);
                }

                var targetDataDiwarehouse = Do.With.Timeout(90).Until(() => _warehouseApplicationStatusHistory.ALL()
                    .Join(Drive.Data.Warehouse.Db.dw.application)
                        .On(Drive.Data.Warehouse.Db.dw.application.ApplicationSK == _warehouseApplicationStatusHistory.ApplicationSK)
                    .Join(Drive.Data.Warehouse.Db.dw.ApplicationStatus)
                        .On(Drive.Data.Warehouse.Db.dw.ApplicationStatus.ApplicationStatusSK == _warehouseApplicationStatusHistory.ApplicationStatusSK)
                    .Where(Drive.Data.Warehouse.Db.dw.application.ApplicationNK == sourceDataRow.ApplicationId.ToString()
                        && Drive.Data.Warehouse.Db.dw.ApplicationStatus.ApplicationStatusNK == sourceDataRow.CurrentStatus)
                    .Select(
                        _warehouseApplicationStatusHistory.ApplicationStatusHistoryNK,
                        Drive.Data.Warehouse.Db.dw.application.ApplicationNK,
                        Drive.Data.Warehouse.Db.dw.ApplicationStatus.ApplicationStatusNK,
                        Drive.Data.Warehouse.Db.dw.ApplicationStatus.ApplicationStatusSK,
                        Drive.Data.Warehouse.Db.dw.Application.ApplicationStatusSK.As("Application_ApplicationStatus")))
                    ;

                //Assert
                Assert.IsNotNull(targetDataDiwarehouse);
                foreach (var targetDataDiwarehouseRow in targetDataDiwarehouse)
                {
                    Assert.AreEqual(sourceDataRow.ApplicationStatusHistoryId.ToString(), targetDataDiwarehouseRow.ApplicationStatusHistoryNK.ToString());
                    Assert.AreEqual(sourceDataRow.CurrentStatus.ToString(), targetDataDiwarehouseRow.ApplicationStatusNK.ToString());
                    Assert.AreEqual(sourceDataRow.ApplicationId.ToString().ToUpper(), targetDataDiwarehouseRow.ApplicationNK.ToString().ToUpper());
                    if (_count == lastItem)
                    {
                        Assert.AreEqual(targetDataDiwarehouseRow.ApplicationStatusSK, targetDataDiwarehouseRow.Application_ApplicationStatus);
                    }
                }
                _count++;
            }
        }

        #region "More Status to application like Complaint,Hardship etc"
        ///// <summary>
        ///// 1. Live application can go into Complaints.
        ///// </summary>
        //[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), JIRA("DI-593"), Owner(Owner.JimmyJose)]
        //[Category("Acceptance Test")]
        //[Description("Verifies the application status when it goes into Complaints and back to Live")]
        //public void ApplicationInComplaintStatusHistory()
        //{
        //    Customer cust = CustomerBuilder.New().Build();
        //    Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

        //    // Make a complaint
        //    var cmd = new CsReportComplaintCommand()
        //    {
        //        AccountId = application.AccountId,
        //        ApplicationId = application.Id,
        //        CaseId = Guid.NewGuid()
        //    };
        //    Drive.Cs.Commands.Post(cmd);

        //When a application is moved to complaints the following tables are updated
        //[Comms].[comms].[Suppressions]
        //[Payments].[payment].[PaymentCollectionSuppressions]

        //TODO: Need to check how Datawarehouse interprets the applicaions in complaint
        //}


        ///// <summary>
        ///// 1. Live application can go into Complaints.
        ///// 2. The application in Complaint goes to Arrear.
        ///// 3. Complaint is removed from the application.
        ///// 4. Application back in Arrear.
        ///// </summary>
        //[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), JIRA("DI-593"), Owner(Owner.JimmyJose)]
        //[Category("Acceptance Test")]
        //[Description("Verifies the application status when it goes into Complaints and back to Live")]
        //public void RemoveComplaintForPreviouslyLiveApplicationThatWentIntoArrearsStatusHistory()
        //{
        //    var caseId = Guid.NewGuid();
        //    Customer cust = CustomerBuilder.New().Build();
        //    Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

        //    // Make a complaint
        //    var cmd = new CsReportComplaintCommand()
        //                  {
        //                      AccountId = application.AccountId,
        //                      ApplicationId = application.Id,
        //                      CaseId = caseId
        //                  };
        //    Drive.Cs.Commands.Post(cmd);

        //    //Puts application into Arrears
        //    application.PutIntoArrears(2);

        //    //Remove Complaint
        //    var removeComplaint = new CsRemoveComplaintCommand()
        //    {
        //        AccountId = application.AccountId,
        //        ApplicationId = application.Id,
        //        CaseId = caseId,
        //    };
        //    Drive.Cs.Commands.Post(removeComplaint);

        //    //TODO: Need to check how Datawarehouse interprets the applicaions in complaint

        //}

        ///// <summary>
        ///// 1. Live application can go into Bankruptcy.
        ///// </summary>
        //[Test]
        //[Category("Acceptance Test")]
        //[Description("Verifies the application status when it goes into Bankruptcy")]
        //public void ApplicationInBankruptcyStatusHistory()
        //{
        //    Customer cust = CustomerBuilder.New().Build();
        //    Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

        //    // report bankruptcy
        //    var cmd = new CsReportBankruptcyCommand()
        //    {
        //        AccountId = application.AccountId,
        //        ApplicationId = application.Id,
        //        CaseId = Guid.NewGuid()
        //    };
        //    Drive.Cs.Commands.Post(cmd);

        //    //TODO: Need to check how Datawarehouse interprets the applicaions in complaint

        //}

        ///// <summary>
        ///// 1. Live application can go into Hardship.
        ///// </summary>
        //[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), JIRA("DI-593"), Owner(Owner.JimmyJose)]
        //[Category("Acceptance Test")]
        //[Description("Verifies the application status when it goes into Hardship")]
        //public void ApplicationInHardshipStatusHistory()
        //{
        //    Customer cust = CustomerBuilder.New().Build();
        //    Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

        //    application.PutIntoArrears(50);

        //    // report hardship
        //    var cmd = new CsReportHardshipCommand()
        //    {
        //        AccountId = application.AccountId,
        //        ApplicationId = application.Id,
        //        CaseId = Guid.NewGuid()
        //    };
        //    Drive.Cs.Commands.Post(cmd);

        //    var cmd1 = new WriteOffApplicationCommand()
        //    {
        //        ApplicationId = application.Id,
        //        DoNotRelend = true
        //    };
        //    Drive.Cs.Commands.Post(cmd1);



        //    //TODO: Need to check how Datawarehouse interprets the applicaions in complaint

        //}

        ///// <summary>
        ///// 1. Live application can go into ManagementReview.
        ///// 2. Application in ManagementReview goes back to Live.
        ///// </summary>
        //[Test, AUT(AUT.Ca, AUT.Uk, AUT.Za), JIRA("DI-593"), Owner(Owner.JimmyJose)]
        //[Category("Acceptance Test")]
        //[Description("Verifies the application status when it goes into ManagementReview")]
        //public void ApplicationInManagementReviewStatusHistory()
        //{
        //    Customer cust = CustomerBuilder.New().Build();
        //    Application application = ApplicationBuilder.New(cust).WithExpectedDecision(ApplicationDecisionStatus.Accepted).Build();

        //    // Make a complaint
        //    var cmd = new CsReportManagementReviewCommand()
        //    {
        //        AccountId = application.AccountId,
        //        ApplicationId = application.Id,
        //        CaseId = Guid.NewGuid()
        //    };
        //    Drive.Cs.Commands.Post(cmd);

        //    //TODO: Need to check how Datawarehouse interprets the applicaions in complaint

        //}

        #endregion "More Status to application like Complaint,Hardship etc"
    }
}