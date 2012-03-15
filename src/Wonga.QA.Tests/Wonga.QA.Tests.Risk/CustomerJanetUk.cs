

using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk
{
	public class CustomerJanetUk : CustomerData
	{
		public CustomerJanetUk()
		{
			ForeName = "Janet";
			MiddleName = "";
			SurName = "Bernadette";
			DateOfBirth = new Date(new DateTime(1963, 07, 26));
			Flat = "1";
			HouseNumber = "14";
			HouseName = "";
			Street = "Mather Avenue";
			District = "Weston Point";
			Town = "Runcorn";
			County = "Cheshire";
			Postcode = "WA7 4JJ";
			CountryCode = "UK";
		}
	}
}
