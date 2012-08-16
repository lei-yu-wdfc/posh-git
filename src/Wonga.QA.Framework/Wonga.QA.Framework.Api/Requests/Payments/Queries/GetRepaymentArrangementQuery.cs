using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetRepaymentArrangement")]
	public partial class GetRepaymentArrangementQuery : ApiRequest<GetRepaymentArrangementQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
