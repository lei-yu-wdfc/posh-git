using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetRepaymentArrangementAllowed")]
	public partial class GetRepaymentArrangementAllowedQuery : ApiRequest<GetRepaymentArrangementAllowedQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
