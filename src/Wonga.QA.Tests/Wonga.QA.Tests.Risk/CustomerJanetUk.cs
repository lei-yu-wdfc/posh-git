

using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk
{
	public class CustomerJanetUk : CustomerData
	{
		public CustomerJanetUk()
		{
			ForeName = "Janet";
			MiddleName = "Bernadette";
			SurName = "";
			DateOfBirth = new Date(new DateTime(1963, 07, 26));
			HouseNumber = "14";
			Street = "Mather Avenue";
			District = "Weston Point";
			Town = "Runcorn";
			County = "Cheshire";
			Postcode = "WA7 4JJ";
			CountryCode = "UK";
		}
	}
}
