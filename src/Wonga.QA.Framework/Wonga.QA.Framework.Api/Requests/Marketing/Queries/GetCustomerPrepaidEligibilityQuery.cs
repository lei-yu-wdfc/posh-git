using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Queries
{
	[XmlRoot("GetCustomerPrepaidEligibility")]
	public partial class GetCustomerPrepaidEligibilityQuery : ApiRequest<GetCustomerPrepaidEligibilityQuery>
	{
		public Object AccountId { get; set; }
	}
}
