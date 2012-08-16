using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
	[XmlRoot("GetCustomerHasActivePrePaidCard")]
	public partial class GetCustomerHasActivePrePaidCardQuery : ApiRequest<GetCustomerHasActivePrePaidCardQuery>
	{
		public Object AccountId { get; set; }
	}
}
