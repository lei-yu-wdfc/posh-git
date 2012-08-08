using System;
using System.Globalization;
using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Builders.Consumer
{
	public class ConsumerAccountDataBase
	{
		public String Password;
		public GenderEnum Gender;
		public String NationalNumber;
		public Date DateOfBirth;
		public String Forename;
		public String MiddleName;
		public String Surname;
		public String MaidenName;
		public UInt16 NumberOfDependants;

		public String Email;
		public String HomePhoneNumber;
		public String MobilePhoneNumber;

		public String Flat;
		public String HouseNumber;
		public String HouseName;
		public String Street;
		public String District;
		public String Town;
		public String County;
		public String Postcode;
		public String CountryCode;
		public ProvinceEnum Province;

		public String EmployerName;
		public String EmploymentStatus;
		public Decimal NetMonthlyIncome;
		public Date NextPayDate;
		public IncomeFrequencyEnum IncomeFrequency;

		public String BankAccountNumber;
		public String BranchNumber;
		public String BankCode;
		public Int64 PaymentCardNumber;
		public String PaymentCardSecurityCode;
		public String PaymentCardType;


		public ConsumerAccountDataBase()
		{
			Password = Get.GetPassword();
			Gender = GenderEnum.Female;
			DateOfBirth = Get.GetDoB();
			Forename = Get.GetName();
			MiddleName = Get.GetName();
			Surname = Get.GetName();
			MaidenName = Gender == GenderEnum.Female ? Get.GetName() : null;
			NumberOfDependants = 0;

			Email = Get.RandomEmail();
			HomePhoneNumber = Get.GetPhone();
			MobilePhoneNumber = Get.GetMobilePhone();

			Flat = Get.RandomString(4);
			HouseNumber = Get.RandomAlphaNumeric(1, 100).ToString(CultureInfo.InvariantCulture);
			HouseName = Get.RandomString(8);
			Street = Get.RandomString(8);
			District = Get.RandomString(8);
			Town = Get.RandomString(8);
			County = Get.RandomString(8);
			Postcode = Get.GetPostcode();
			CountryCode = Get.GetCountryCode();
			Province = ProvinceEnum.ON;

			EmployerName = Get.GetEmployerName();
			EmploymentStatus = Get.GetEmploymentStatus();
			NextPayDate = Get.GetNextPayDate();
			IncomeFrequency = IncomeFrequencyEnum.LastFridayOfMonth;

			BankAccountNumber = Get.GetBankAccountNumber();
			BranchNumber = "00018";
			PaymentCardNumber = 4444333322221111;
			PaymentCardSecurityCode = "777"; ;
			PaymentCardType = "Visa";
		}
	}
}
