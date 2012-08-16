using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetAvailableCredit")]
	public partial class GetAvailableCreditUkQuery : ApiRequest<GetAvailableCreditUkQuery>
	{
		public Object AccountId { get; set; }
	}
}
