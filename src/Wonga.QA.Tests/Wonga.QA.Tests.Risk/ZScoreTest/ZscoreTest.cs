﻿using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;
using Wonga.QA.Tests.Risk.Properties;

namespace Wonga.QA.Tests.Risk.ZScoreTest
{
    class ZScoreTest
    {
        public const string CallReportMockMode = "Mocks.CallReportEnabled";
        private string _savedCallReportMockMode;
        [SetUp]
        public void Setup()
        {
            _savedCallReportMockMode = Drive.Db.GetServiceConfiguration(CallReportMockMode).Value;
            SetCallReportTrialMode(false);
        }

        [TearDown]
        public void TearDown()
        {
            Drive.Db.SetServiceConfiguration(CallReportMockMode, "true");
        }
        [Test, AUT(AUT.Wb)]
        [JIRA("SME-956"), Description("score card test with CallCredit"), Explicit("This test is disabling CallReport mock and as tests are being run in parallel, this makes other tests fail.")]
        public void ScoreCardTest1000WithCallCredit()
        {
            var data = Resources.CustomerDataforCallCredit;
            var delimiters = new[] { ',', ';' };
            string[] customerdetails = data.Split(delimiters[1]);
            foreach (var customerdetail in customerdetails)
            {
                try
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
                    CreateApplication(forename, surname, dateOfBirth, PostCode, HouseNumber, MiddleName, City, Street);
                }
                catch
                {
                }
                finally
                {
                    Drive.Db.SetServiceConfiguration(CallReportMockMode, "true");
                }
            }
            Drive.Db.SetServiceConfiguration(CallReportMockMode, "true");
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
        public static void SetCallReportTrialMode(Boolean value)
        {
            var db = new DbDriver();
            var bgw = db.Ops.ServiceConfigurations.Single(bg => bg.Key == CallReportMockMode);
            var dbValue = bgw.Value;
            var wantedValue = value.ToString().ToLower();
            if (dbValue != wantedValue)
            {
                bgw.Value = wantedValue;
                db.Ops.SubmitChanges();
            }
        }
    }
}
