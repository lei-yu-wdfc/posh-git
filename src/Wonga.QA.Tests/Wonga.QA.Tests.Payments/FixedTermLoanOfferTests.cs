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
        private const string DateOverrideKey = "Payments.DateOverrideKey.DateTime.UtcNow";
		private static readonly int[] _payDayPerMonth = Driver.Db.GetServiceConfiguration("Payments.PayDayPerMonth").Value.Split(',').Select(Int32.Parse).ToArray();
		private static readonly int[] _payDayPlusToMaxTerm = Driver.Db.GetServiceConfiguration("Payments.PayDayPlusToMaxTerm").Value.Split(',').Select(Int32.Parse).ToArray();
		private static readonly int _sliderTermAddDays = Int32.Parse(Driver.Db.GetServiceConfiguration("Payments.SliderTermAddDays").Value);
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
			Driver.Db.SetServiceConfiguration(DateOverrideKey, new Date(today, DateFormat.Date).ToString());

            var response = Driver.Api.Queries.Post(new GetFixedTermLoanOfferZaQuery());
            
            Assert.AreEqual(defaultTerm, int.Parse(response.Values["TermDefault"].Single()));
            Assert.AreEqual(maxTerm, int.Parse(response.Values["TermMax"].Single()));
        }
	}
}
