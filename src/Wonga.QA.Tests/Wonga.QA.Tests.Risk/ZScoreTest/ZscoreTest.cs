using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Properties;
using System.Threading;
using System.Text;

namespace Wonga.QA.Tests.Risk.ZScoreTest
{
    class ZScoreTest
    {
        public const string CallReportMockMode = "Mocks.CallReportEnabled";

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-956"), Description("score card test with CallCredit")]
        [Explicit("This test is disabling CallReport mock and as tests are being run in parallel, this makes other tests fail.")]
        [Ignore("We can't turn off mocks just only with db settings. No sense in this test")]
        public void ScoreCardTest1000WithCallCredit()
        {
            var sqlQuery = new StringBuilder();
            var data = Resources.CustomerDataforCallCredit;
            var delimiters = new[] { ',', ';' };
            string[] customerdetails = data.Split(delimiters[1]);

            //REMEBER TO DISABLE WAITFORRISK INSIDE BUILD

            foreach (var customerdetail in customerdetails)
            {
                string[] custdata = customerdetail.Split(delimiters[0]);
                DateTime theDateTime = DateTime.Parse(custdata[3]);
                var forename = custdata[0];
                var MiddleName = custdata[1];
                var surname = custdata[2];

                //sqlQuery.Append(" '"+surname + "' , ");

                var dateOfBirth = new Date(theDateTime, DateFormat.Date);
                var HouseNumber = custdata[4];
                var PostCode = custdata[5];
                var Street = custdata[6];
                var City = custdata[7];
                CreateApplication(forename, surname, dateOfBirth, PostCode, HouseNumber, MiddleName, City, Street);
            }

            //var z = sqlQuery;
        }

        [Test, AUT(AUT.Wb)]
        [JIRA("SME-957"), Description("score card test with CallCredit")]
        [Explicit("This test is disabling CallReport mock and as tests are being run in parallel, this makes other tests fail.")]
        [Ignore("We can't turn off mocks just only with db settings. No sense in this test")]
        [Timeout(20000)]
        public void LoadZScoreTestCustomers()
        {
            var sqlQuery = new StringBuilder();
            Console.WriteLine("Begin");
            var data = Resources.ZScoreCustomers;
            var delimiters = new[] { ',', ';' };
            string[] customerdetails = data.Split(delimiters[1]);

            var iterator = customerdetails.Length;

            //ENABLE WAITFORRISK
            //DELETE THREAD.SLEEP WithOrganisationNumber
            Console.WriteLine("\r\n We Have -> " + iterator + " cases. Good luck");

            foreach (var customerdetail in customerdetails)
            {
                string[] custdata = customerdetail.Split(delimiters[0]);
                DateTime theDateTime = DateTime.Parse(custdata[3]);
                var forename = custdata[0];
                var MiddleName = custdata[1];
                var surname = custdata[2];
                var dateOfBirth = new Date(theDateTime, DateFormat.Date);
                var HouseNumber = custdata[4];
                var PostCode = custdata[5];
                var Street = custdata[6];
                var City = custdata[7];

                var mainApplicantSpecificSurname = "AlexZ" + Get.RandomString(6);

                //sqlQuery.Append(" '" + surname + "' , ");

                Console.WriteLine("Doing it for -> " + forename + " " + surname);
                Console.WriteLine("\r\n " + iterator-- + " cases left!");
                CreateApplicationWithGuarantor(forename, surname, dateOfBirth, PostCode, HouseNumber, MiddleName, City, Street,mainApplicantSpecificSurname);
                Thread.Sleep(1000);
            }

            //var result = sqlQuery.ToString();
        }

        private static void CreateApplication(string forename, string surname, Date dateOfBirth, string postCode, string houseNumber, string middleName, string city, string street)
        {
            var customerBuilder = CustomerBuilder.New();
            customerBuilder.ScrubForename(forename);
            customerBuilder.ScrubSurname(surname);
            var customer =
                customerBuilder.WithDateOfBirth(dateOfBirth).WithForename(forename).WithSurname(surname).
                    WithPostcodeInAddress(postCode).WithHouseNumberInAddress(houseNumber).
                    WithStreetInAddress(street).WithTownInAddress(city).Build();


            var organization = OrganisationBuilder.New(customer).Build();
            ApplicationBuilder.New(customer, organization).Build();
            return;
        }

        private static void CreateApplicationWithGuarantor(string forename, string surname, Date dateOfBirth, string postCode, string houseNumber, string middleName, string city, string street,string mainApplicantSpecificSurname)
        {
            var guarantorBuilder = CustomerBuilder.New();
            var mobilePhoneNumber = Get.GetMobilePhone();

            guarantorBuilder.ScrubForename(forename);
            guarantorBuilder.ScrubSurname(surname);
            var guarantor =
                guarantorBuilder.WithDateOfBirth(dateOfBirth).WithForename(forename).WithSurname(surname).
                    WithPostcodeInAddress(postCode).WithMiddleName(middleName).WithHouseNumberInAddress(houseNumber).
                    WithStreetInAddress(street).WithTownInAddress(city).WithMobileNumber(mobilePhoneNumber);

            var mainApplicant = CustomerBuilder.New().WithSurname(mainApplicantSpecificSurname).Build();
            var organization = OrganisationBuilder.New(mainApplicant).Build();
            var application = ApplicationBuilder.New(mainApplicant, organization) as BusinessApplicationBuilder;
            application.WithGuarantors(new List<CustomerBuilder> { guarantor }).Build();
        }
    }
}
