using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.MigrationTests.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.MigrationTests
{
    [TestFixture]
    //[Parallelizable(TestScope.All)]
    class MigrationApiTests
    {
        private int _batchId = 1;
        private MigratedUser _migratedUser;
        private readonly MigrationHelper _migHelper = new MigrationHelper();
        private byte _loginStatus = new byte();
        private string _testName;
        private DateTime _testStartTime;
        private DateTime _testEndTime;

        private string GetFunctionName()
        {
            var stackTrace = new StackTrace();
            var stackFrame = stackTrace.GetFrame(1);
            var methodBase = stackFrame.GetMethod();

            return methodBase.Name;
        }

        public void StoreTestResults()
        {
            _migHelper.StoreTestResults(_batchId.ToString(CultureInfo.InvariantCulture), _testName,
                                                   _migratedUser, _testStartTime, _testEndTime, _loginStatus);
        }

        [FixtureSetUp]
        public void FixtureSetup()
        {
            //if (_migHelper.IsControlTableEmpty())
            //{
            //    _migHelper.FillAcceptanceTestControlTable();
            //}
            _testStartTime = DateTime.Now;
        }
        [Test, JIRA("UKMIG-226"),/*Parallelizable,*/ Owner(Owner.MuhammadQureshi)]
        public void MigratedApiLnJournery()
        {
            _testName = GetFunctionName();
            _testStartTime = DateTime.Now;
            //_migratedUser = new MigratedUser();
            //_migratedUser = _migHelper.GetMigratedAccountLogin();
            var customer = new Customer(Guid.Parse
                                            (Drive.Api.Queries.Post
                                                 //(new GetAccountQuery { Login = _migratedUser.Login, Password = _migratedUser.Password }).
                (new GetAccountQuery{ Login = "qa.wonga.com+BUILD-WIN21-7af66e12-7664-4bec-8a51-f5ffb17f41b2@gmail.com", Password = "Passw0rd"}).
                                                 Values["AccountId"].Single()));

            //var promiseDate = new Date(DateTime.Now.AddDays(10));
            //var loanAmmount = new decimal(1000);

            //ApplicationBuilder.New(customer,promiseDate,loanAmmount).Build();
            ApplicationBuilder.New(customer).Build();
            //ApplicationBuilder.New(customer).Build().RepayOnDueDate();

            _testEndTime = DateTime.Now;
            //StoreTestResults();
        }

        [Test, JIRA("UKMIG-231"),/*Parallelizable,*/ Owner(Owner.MuhammadQureshi)]
        public void MigratedApiLnJourneryAndRepayOnDueDate()
        {
            _testName = GetFunctionName();
            _testStartTime = DateTime.Now;
            //_migratedUser = new MigratedUser();
            //_migratedUser = _migHelper.GetMigratedAccountLogin();
            var customer = new Customer(Guid.Parse
                                            (Drive.Api.Queries.Post
                //(new GetAccountQuery { Login = _migratedUser.Login, Password = _migratedUser.Password }).
                (new GetAccountQuery { Login = "qa.wonga.com+BUILD-WIN21-7af66e12-7664-4bec-8a51-f5ffb17f41b2@gmail.com", Password = "Passw0rd" }).
                                                 Values["AccountId"].Single()));

            //var promiseDate = new Date(DateTime.Now.AddDays(10));
            //var loanAmmount = new decimal(1000);

            //ApplicationBuilder.New(customer,promiseDate,loanAmmount).Build();
            ApplicationBuilder.New(customer).Build().RepayOnDueDate();

            _testEndTime = DateTime.Now;
            //StoreTestResults();
        }

        [Test, JIRA("UKMIG-231"),/*Parallelizable,*/ Owner(Owner.MuhammadQureshi)]
        public void MigratedApiLnJourneryAndRepayEarly()
        {
            _testName = GetFunctionName();
            _testStartTime = DateTime.Now;
            //_migratedUser = new MigratedUser();
            //_migratedUser = _migHelper.GetMigratedAccountLogin();
            var customer = new Customer(Guid.Parse
                                            (Drive.Api.Queries.Post
                //(new GetAccountQuery { Login = _migratedUser.Login, Password = _migratedUser.Password }).
                (new GetAccountQuery { Login = "qa.wonga.com+BUILD-WIN21-7af66e12-7664-4bec-8a51-f5ffb17f41b2@gmail.com", Password = "Passw0rd" }).
                                                 Values["AccountId"].Single()));

            var promiseDate = new Date(DateTime.Now.AddDays(30));
            var loanAmmount = new decimal(300);

            var lnApplication = ApplicationBuilder.New(customer,promiseDate,loanAmmount).Build();

            var repayAmmount = new decimal(100);

            lnApplication.RepayEarly(repayAmmount, 15);
            //ApplicationBuilder.New(customer, promiseDate, loanAmmount).Build().RepayEarly(repayAmmount,2);

            _testEndTime = DateTime.Now;
            //StoreTestResults();
        }
    }
}
