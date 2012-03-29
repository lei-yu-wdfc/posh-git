using System;
using System.Linq;
using MbUnit.Framework;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Api;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Extensions;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Payments
{
    public class FixedTermLoanOfferTests
    {
		private const string DateOverrideKey = "Payments.FixedTermLoanOfferHandler.DateTime.UtcNow";
		private static readonly int[] PayDayPerMonth = Drive.Db.GetServiceConfiguration("Payments.PayDayPerMonth").Value.Split(',').Select(Int32.Parse).ToArray();
		private static readonly int[] PayDayPlusToMaxTerm = Drive.Db.GetServiceConfiguration("Payments.PayDayPlusToMaxTerm").Value.Split(',').Select(Int32.Parse).ToArray();
		private static readonly int SliderTermAddDays = Int32.Parse(Drive.Db.GetServiceConfiguration("Payments.SliderTermAddDays").Value);
    	private const int MaxDefaultTerm = 30;

		[FixtureTearDown]
		public void TearDownOverride()
		{
			var driver = new DbDriver();
			var scEntry = driver.Ops.ServiceConfigurations.SingleOrDefault(x => x.Key == DateOverrideKey);
			if (scEntry != null)
			{
				driver.Ops.ServiceConfigurations.DeleteOnSubmit(scEntry);
				driver.Ops.SubmitChanges();
			}
		}

        [Test, AUT(AUT.Za), JIRA("ZA-2024")]
        [Row("2012-2-23", 30, 36, Order = 0)]
        [Row("2012-3-1", 23, 30, Order = 1)]
        [Row("2012-8-1", 24, 30, Order = 2)]
        public void GetFixedTermLoanOfferTest(DateTime today, int defaultTerm, int maxTerm)
        {
			Drive.Db.SetServiceConfiguration(DateOverrideKey, new Date(today, DateFormat.Date).ToString());

            var response = Drive.Api.Queries.Post(new GetFixedTermLoanOfferZaQuery());
            
            Assert.AreEqual(defaultTerm, int.Parse(response.Values["TermDefault"].Single()));
            Assert.AreEqual(maxTerm, int.Parse(response.Values["TermMax"].Single()));
        }

		[Test, AUT(AUT.Za), Explicit("Release")]
		public void MaxLoanTermIsLastDayOfMonth()
		{
			for(int i = 0; i < 12; i++)
			{
				var payDayOfMonth = PayDayPerMonth[i];
				var payDayPlusToMaxTerm = PayDayPlusToMaxTerm[i];

				var now = new DateTime(DateTime.UtcNow.Year, i + 1, 1);
				Drive.Db.SetServiceConfiguration(DateOverrideKey, now.ToString("yyyy-MM-dd HH:mm:ss"));

				var response = Drive.Api.Queries.Post(new GetFixedTermLoanOfferZaQuery());

				var expectedMax = payDayOfMonth + payDayPlusToMaxTerm;
				Assert.AreEqual(expectedMax, DateTime.DaysInMonth(now.Year, now.Month));

				var actualMax = int.Parse(response.Values["TermMax"].Single());
				Assert.AreEqual(expectedMax, actualMax);
			}
		}
	}
}
