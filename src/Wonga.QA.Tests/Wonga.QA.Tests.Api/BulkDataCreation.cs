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

        [Test, AUT(AUT.Uk), JIRA("QA-320"), Ignore]
        public void Create2000CustomersWithProperInfo()
        {
            Random r = new Random();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@"..\src\Wonga.QA.Tests\Wonga.QA.Tests.Api\CustomerInfo.xml");
            XmlNodeList femaleName = xmlDoc.GetElementsByTagName("FemaleName");
            XmlNodeList maleName = xmlDoc.GetElementsByTagName("MaleName");
            XmlNodeList surName = xmlDoc.GetElementsByTagName("LastName");
            XmlNodeList street = xmlDoc.GetElementsByTagName("StreetName");
            XmlNodeList country = xmlDoc.GetElementsByTagName("CounrtyName");
            XmlNodeList town = xmlDoc.GetElementsByTagName("CityName");
            XmlNodeList district = xmlDoc.GetElementsByTagName("DistrictName");
            for (int i = 0; i < 100; i++)
            {
                if (i % 2 == 0)
                {
                    Customer customer = CustomerBuilder.New()
                        .WithGender(GenderEnum.Male)
                        .WithForename(maleName[r.Next(0, 4900)].InnerText)
                        .WithSurname(surName[r.Next(0, 9000)].InnerText)
                        .WithStreetInAddress(street[r.Next(0, 635)].InnerText)
                        .WithFlatInAddress(i.ToString())
                        .WithTownInAddress(town[r.Next(0, 45)].InnerText)
                        .WithDistrictInAddress(district[r.Next(0, 23)].InnerText)
                        .WithCountyInAddress(country[r.Next(0, 45)].InnerText).Build();
                 }
                else
                {
                    Customer customer = CustomerBuilder.New()
                        .WithGender(GenderEnum.Female)
                        .WithForename(femaleName[r.Next(0, 4385)].InnerText)
                        .WithSurname(surName[r.Next(0, 9000)].InnerText)
                        .WithStreetInAddress(street[r.Next(0, 630)].InnerText)
                        .WithFlatInAddress(i.ToString())
                        .WithTownInAddress(town[r.Next(0, 45)].InnerText)
                        .WithDistrictInAddress(district[r.Next(0, 23)].InnerText)
                        .WithCountyInAddress(country[r.Next(0, 45)].InnerText).Build();
                }
            }
        }
    }
}
