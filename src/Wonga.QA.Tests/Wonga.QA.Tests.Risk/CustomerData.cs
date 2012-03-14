using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Risk
{
	public class CustomerData : ICustomerData
	{
		public string ForeName { get; set; }
		public string MiddleName { get; set; }
		public string SurName { get; set; }
		public Date DateOfBirth { get; set; }
		public string Flat { get; set; }
		public string HouseNumber { get; set; }
		public string HouseName { get; set; }
		public string Street { get; set; }
		public string District { get; set; }
		public string Town { get; set; }
		public string County { get; set; }
		public string Postcode { get; set; }
		public string CountryCode { get; set; }
	}
}
