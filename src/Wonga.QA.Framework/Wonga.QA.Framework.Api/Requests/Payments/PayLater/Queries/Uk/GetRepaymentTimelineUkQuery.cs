using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetRepaymentTimeline")]
	public partial class GetRepaymentTimelineUkQuery : ApiRequest<GetRepaymentTimelineUkQuery>
	{
		public Object AccountId { get; set; }
	}
}
