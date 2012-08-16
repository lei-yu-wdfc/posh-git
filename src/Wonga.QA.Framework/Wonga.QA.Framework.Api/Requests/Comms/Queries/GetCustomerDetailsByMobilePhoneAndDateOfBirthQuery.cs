using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
	[XmlRoot("GetCustomerDetailsByMobilePhoneAndDateOfBirth")]
	public partial class GetCustomerDetailsByMobilePhoneAndDateOfBirthQuery : ApiRequest<GetCustomerDetailsByMobilePhoneAndDateOfBirthQuery>
	{
		public Object DateOfBirth { get; set; }
		public Object MobilePhone { get; set; }
	}
}
