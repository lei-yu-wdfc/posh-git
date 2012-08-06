using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Requests.Ops.Queries;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI;
using Wonga.QA.Framework.UI.UiElements.Pages.Common;
using Wonga.QA.Framework.Data;
using Wonga.QA.Tests.Core;
using Wonga.QA.UiTests.Web;

namespace Wonga.QA.MigrationTests
{
    [TestFixture]
    //[Parallelizable(TestScope.All)]
    public class MigrationUiTests : UiTest
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
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();

            return methodBase.Name;
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

        public void StoreTestResults()
        {
            _migHelper.StoreTestResults(_batchId.ToString(CultureInfo.InvariantCulture), _testName,
                                                   _migratedUser, _testStartTime, _testEndTime, _loginStatus);
        }

        [Test, MultipleAsserts, JIRA("UKMIG-243"), /*Parallelizable,*/ Owner(Owner.MuhammadQureshi)]
        [Row(10, "2012")]
        [Row(10, "2011")]
        [Row(10, "2010")]
        [Row(10, "2010")]
        [Row(10, "2010")]
        public void TestMigratedUserCanLogin(int noOfTimesToRun, string userCreatedInYear)
        {
            _testName = GetFunctionName();

            //migHelper.FillAcceptanceTestControlTable();

            Console.WriteLine("UKMIG-243, As an existing migrated customer, I want to login to the Wonga website so that I can manage my account or apply for a new loan, " + _testName);

            for (int usersToCheck = 1; usersToCheck < noOfTimesToRun; usersToCheck++)
            {
                _testStartTime = DateTime.Now;
                _migratedUser = new MigratedUser();
                //_migratedUser = _migHelper.GetMigratedAccountLogin();

                //var migratedAccountLoginPassword = migHelper.GetMigratedAccountLoginPassword(migratedAccountLogin);


                using (var loginPage = new UiClient())
                {
                    try
                    {
                        //loginPage.Login().LoginAs(_migratedUser.Login, _migratedUser.Password);
                        _loginStatus = 1;
                    }
                    catch (Exception)
                    {
                        _loginStatus = 0;
                    }
                    finally
                    {
                        _testEndTime = DateTime.Now;
                        StoreTestResults();
                        //Console.WriteLine("\n{0}: {1} Login = {2} \nUser Created in Year = {3}", usersToCheck,
                        //                loginStatus, migratedUser, userCreatedInYear);
                        //_migHelper.StoreTestResults(_batchId.ToString(CultureInfo.InvariantCulture), _testName,
                        //                           migratedUser, _testStartTime, _testEndTime, _loginStatus);
                    }
                }
            }
        }


        [Test, AUT(AUT.Uk), Owner(Owner.MuhammadQureshi)]
        [Row(2, "2010")]
        public void TestMigratedUserCanTakeANewLoan(int noOfTimesToRun, string userCreatedInYear)
        {
            _testName = GetFunctionName();

            //migHelper.FillAcceptanceTestControlTable();

            Console.WriteLine(
                "UKMIG-226, As an existing migrated customer, I want to take out a new loan so that I can get extra cash when I need it, " +
                _testName);

            for (int usersToCheck = 1; usersToCheck < noOfTimesToRun; usersToCheck++)
            {
                _testStartTime = DateTime.Now;
                _migratedUser = new MigratedUser();
                _migratedUser = _migHelper.GetMigratedAccountLogin(null, null);

                using (var loginPage = new UiClient())
                {
                    try
                    {
                        loginPage.Login().LoginAs(_migratedUser.Login, _migratedUser.Password);
                        var journey = JourneyFactory.GetLnJourney(Client.Home());
                        var page = journey.Teleport<MySummaryPage>() as MySummaryPage;
                        _loginStatus = 1;
                    }
                    catch (Exception)
                    {
                        _loginStatus = 0;
                    }
                    finally
                    {
                        _testEndTime = DateTime.Now;
                        StoreTestResults();
                        //Console.WriteLine("\n{0}: {1} Login = {2} \nUser Created in Year = {3}", usersToCheck,
                        //                loginStatus, migratedUser, userCreatedInYear);
                        //_migHelper.StoreTestResults(_batchId.ToString(CultureInfo.InvariantCulture), _testName,
                        //                           migratedUser, _testStartTime, _testEndTime, _loginStatus);
                    }
                }

            }
        }

        [Test]
        public void MigratedApiLnJournery()
        {
            _migratedUser = new MigratedUser();
            _migratedUser = _migHelper.GetMigratedAccountLogin();
            var customer = new Customer(Guid.Parse
                                            (Drive.Api.Queries.Post
                                                 (new GetAccountQuery { Login = _migratedUser.Login, Password = _migratedUser.Password }).
                                                 //{ Login = "qa.wonga.com+BUILD-WIN21-7af66e12-7664-4bec-8a51-f5ffb17f41b2@gmail.com", Password = "Passw0rd"}).//
                                                 Values["AccountId"].Single()));
            
            //var promiseDate = new Date(DateTime.Now.AddDays(10));
            //var loanAmmount = new decimal(1000);

            //ApplicationBuilder.New(customer,promiseDate,loanAmmount).Build();
            ApplicationBuilder.New(customer).Build();
        }

        [Test]
        public void CreateUser()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();//"testMigratedUser@gmail.com";

            Customer customer = CustomerBuilder
                .New()
                .WithEmailAddress(email)
                .Build();

            Application application = ApplicationBuilder
                .New(customer)
                .Build();

            loginPage.LoginAs(email);

        }

        [Test]
        public void CreateUserFromWeb()
        {
            var loginPage = Client.Login();
            string email = Get.RandomEmail();
            Console.WriteLine("email={0}", email);

            // L0 journey
            var journeyL0 = JourneyFactory.GetL0Journey(Client.Home())
                .WithEmployerName(Get.EnumToString(RiskMask.TESTEmployedMask)).WithEmail(email);
            var mySummary = journeyL0.Teleport<MySummaryPage>() as MySummaryPage;
        }

    }
}
