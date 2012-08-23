using System;
using System.Globalization;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.PayLater
{
	public class PayLaterAccountDataBase
	{
		public String Password;
		public Date DateOfBirth;
		public String Forename;
		public String Surname;

		public String Email;
		public String MobilePhoneNumber;

		public String Flat;
		public String HouseNumber;
		public String Street;
		public String Town;
		public String County;
		public String Postcode;
		public String CountryCode;

		public EmploymentStatusEnum EmploymentStatus;
		public Decimal NetMonthlyIncome;
		public Date NextPayDate;
		public IncomeFrequencyEnum IncomeFrequency;

		public Int64 PaymentCardNumber;
		public String PaymentCardSecurityCode;
		public Date PaymentCardExpiryDate;

		public PayLaterAccountDataBase()
		{
			Password = Get.GetPassword();
			DateOfBirth = Get.GetDoB();
			Forename = Get.GetName();
			Surname = Get.GetName();

			Email = Get.RandomEmail();
			MobilePhoneNumber = Get.GetMobilePhone();

			Flat = Get.RandomString(4);
			HouseNumber = Get.RandomAlphaNumeric(1, 100).ToString(CultureInfo.InvariantCulture);
			Street = Get.RandomString(8);
			Town = Get.RandomString(8);
			County = Get.RandomString(8);
			Postcode = Get.GetPostcode();
			CountryCode = Get.GetCountryCode();

			EmploymentStatus = (EmploymentStatusEnum) Enum.Parse(typeof (EmploymentStatusEnum), Get.GetEmploymentStatus());
			NextPayDate = Get.GetNextPayDate();
			IncomeFrequency = IncomeFrequencyEnum.LastFridayOfMonth;

			PaymentCardNumber = 4444333322221111;
			PaymentCardSecurityCode = "777";
            PaymentCardExpiryDate = DateTime.UtcNow.AddYears(2).ToDate(DateFormat.YearMonth);
		}
	}
}
