using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk
{
	public interface ICustomerData
	{
		string ForeName { get; set; }
		string MiddleName { get; set; }
		string SurName { get; set; }
		Date DateOfBirth { get; set; }

		string Postcode { get; set; }
		string Flat { get; set; }
		string HouseNumber { get; set; }
		string HouseName { get; set; }
		string Street { get; set; }
		string District { get; set; }
		string Town { get; set; }
		string County { get; set; }
		string CountryCode { get; set; }
	}
}
