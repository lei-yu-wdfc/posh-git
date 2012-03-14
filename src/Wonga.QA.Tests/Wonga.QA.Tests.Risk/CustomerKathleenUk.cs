using System;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk
{
	public class CustomerKathleenUk : CustomerData
	{
		public CustomerKathleenUk()
		{
			ForeName = "Kathleen";
			MiddleName = "A";
			SurName = "Martin";
			DateOfBirth = new Date(new DateTime(1987, 09, 18));
			Flat = "10";
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
