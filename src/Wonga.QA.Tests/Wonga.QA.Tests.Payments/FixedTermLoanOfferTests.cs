using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class FixedTermLoanOfferTests
    {
        private const string FixedTermLoanOfferHandler = "Payments.FixedTermLoanOfferHandler.DateTime.UtcNow";

        [Test, AUT(AUT.Za), JIRA("ZA-2024")]
        [Row("2012-2-23", 30, 36, Order = 0)]
        [Row("2012-3-1", 23, 30, Order = 1)]
        [Row("2012-8-1", 24, 30, Order = 2)]
        public void GetOfferTest(DateTime today, int defaultTerm, int maxTerm)
        {
            SetupQaUtcNowOverride(today);
            var response = Driver.Api.Queries.Post(new GetFixedTermLoanOfferZaQuery());
            
            Assert.AreEqual(defaultTerm, int.Parse(response.Values["TermDefault"].Single()));
            Assert.AreEqual(maxTerm, int.Parse(response.Values["TermMax"].Single()));
        }

        private void SetupQaUtcNowOverride(DateTime now)
        {
            var driver = new DbDriver();
            var scEntry = driver.Ops.ServiceConfigurations.SingleOrDefault(x => x.Key == FixedTermLoanOfferHandler);
            if (scEntry == null)
            {
                driver.Ops.ServiceConfigurations.InsertOnSubmit(new ServiceConfigurationEntity()
                                                                       {
                                                                           Key = FixedTermLoanOfferHandler,
                                                                           Value = now.Date.ToString("yyyy-MM-dd")
                                                                       });
            }
            else
            {
                scEntry.Value = now.Date.ToString("yyyy-MM-dd");
            }

            driver.Ops.SubmitChanges();
        }

        [FixtureTearDown]
        public void TearDownOverride()
        {
            var driver = new DbDriver();
            var scEntry = driver.Ops.ServiceConfigurations.SingleOrDefault(x => x.Key == FixedTermLoanOfferHandler);
            if (scEntry != null)
            {
                driver.Ops.ServiceConfigurations.DeleteOnSubmit(scEntry);
                driver.Ops.SubmitChanges();
            }
        }
    }
}
