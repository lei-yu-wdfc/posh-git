using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk
{
	public class CustomerLauraUk : CustomerData
	{
		public CustomerLauraUk()
		{
			ForeName = "Laura";
			MiddleName = "";
			SurName = "Insolvent";
			DateOfBirth = new Date(new DateTime(1987, 09, 18));
			Flat = "1";
			HouseNumber = "6";
			HouseName = "1";
			Street = "Mather Avenue";
			District = "";
			Town = "Shanklin";
			County = "";
			Postcode = "BB12 0NL";
			CountryCode = "UK";
		}
	}
}
