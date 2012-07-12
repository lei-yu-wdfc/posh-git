using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Api.Requests.Address.Queries.Uk;
using Wonga.QA.Framework.Api.Requests.Comms.Commands;
using Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk;
using Wonga.QA.Framework.Api.Requests.Comms.Queries;
using Wonga.QA.Framework.Api.Requests.Ops.Commands;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Comms;
using Wonga.QA.Tests.Core;
using Wonga.QA.Framework.Api.Exceptions;

namespace Wonga.QA.Tests.Api
{
    class BulkDataCreation
    {
        Random r = new Random();
        static XmlDocument xmlDoc = new XmlDocument();
        XmlNodeList femaleName = xmlDoc.GetElementsByTagName("FemaleName");
        XmlNodeList maleName = xmlDoc.GetElementsByTagName("MaleName");
        XmlNodeList surName = xmlDoc.GetElementsByTagName("LastName");
        XmlNodeList street = xmlDoc.GetElementsByTagName("StreetName");
        XmlNodeList country = xmlDoc.GetElementsByTagName("CounrtyName");
        XmlNodeList town = xmlDoc.GetElementsByTagName("CityName");
        XmlNodeList district = xmlDoc.GetElementsByTagName("DistrictName");

        [SetUp]
        public void SetUp()
        {
            xmlDoc.Load(@"..\src\Wonga.QA.Tests\Wonga.QA.Tests.Api\CustomerInfo.xml");
        }



        [Test, AUT(AUT.Uk), JIRA("QA-320"), Ignore]
        [Row(225, 1)]
        [Row(225, 2)]
        [Row(225, 3)]
        [Row(200, 5)]
        [Row(175, 10)]
        [Row(150, 30)]
        [Row(120, 60)]
        [Row(75, 90)]
        public void CreateArrearsCustomersWithProperInfo(int customerNum, uint day)
        {
            for (int i = 0; i < customerNum; i++)
            {
                Customer properCustomer = properCustomerCreator();
                Application aplication = ApplicationBuilder.New(properCustomer).Build();
                aplication.PutIntoArrears(day);

            }

        }

        [Test, AUT(AUT.Uk), JIRA("QA-320"), Ignore]
        [Row(82, 20)]
        [Row(125, 10)]
        [Row(225, 1)]
        public void CreateLiveLoanCustomersWithProperInfo(int customersNum, int day)
        {
            for (int i = 0; i < customersNum; i++)
            {
                Customer properCustomer = properCustomerCreator();
                Application application = ApplicationBuilder.New(properCustomer).WithLoanTerm(day).Build();
                
            }

        }

        [Test, AUT(AUT.Uk), JIRA("QA-320"), Ignore]
        public void CreateDueTodayCustomersWithProperInfo()
        {
            for (int i = 0; i < 2; i++)
            {
                Customer properCustomer = properCustomerCreator();
                Application application = ApplicationBuilder.New(properCustomer).Build();
                application.MakeDueToday();

            }

        }

        [Test, AUT(AUT.Uk), JIRA("QA-320"), Ignore]
        [Row(13, -1)]
        [Row(63, 0)]
        [Row(13, 1)]
        [Row(13, 2)]
        [Row(13, 3)]
        [Row(22, 5)]
        [Row(13, 10)]
        [Row(13, 30)]
        [Row(13, 60)]
        [Row(13, 7)]
        public void CreatePaidFullCustomersWithProperInfo(int customersNum, int day)
        {
            for (int i = 0; i < customersNum; i++)
            {
                Customer properCustomer = properCustomerCreator();
                Application application = ApplicationBuilder.New(properCustomer).Build();
                application.RepayEarly(application.GetBalanceToday(), day);

            }

        }

        public Customer properCustomerCreator()
        {

            if (r.Next(1, 3) % 2 == 0)
            {
                Customer customer = CustomerBuilder.New()
                    .WithGender(GenderEnum.Male)
                    .WithForename(maleName[r.Next(0, 4900)].InnerText)
                    .WithSurname(surName[r.Next(0, 9000)].InnerText)
                    .WithStreetInAddress(street[r.Next(0, 635)].InnerText)
                    .WithFlatInAddress(r.Next(0, 100).ToString())
                    .WithTownInAddress(town[r.Next(0, 45)].InnerText)
                    .WithDistrictInAddress(district[r.Next(0, 23)].InnerText)
                    .WithCountyInAddress(country[r.Next(0, 45)].InnerText).Build();
                return customer;
            }
            else
            {
                Customer customer = CustomerBuilder.New()
                    .WithGender(GenderEnum.Female)
                    .WithForename(femaleName[r.Next(0, 4385)].InnerText)
                    .WithSurname(surName[r.Next(0, 9000)].InnerText)
                    .WithStreetInAddress(street[r.Next(0, 630)].InnerText)
                    .WithFlatInAddress(r.Next(0, 100).ToString())
                    .WithTownInAddress(town[r.Next(0, 45)].InnerText)
                    .WithDistrictInAddress(district[r.Next(0, 23)].InnerText)
                    .WithCountyInAddress(country[r.Next(0, 45)].InnerText).Build();
                return customer;
            }

        }

    }
}
