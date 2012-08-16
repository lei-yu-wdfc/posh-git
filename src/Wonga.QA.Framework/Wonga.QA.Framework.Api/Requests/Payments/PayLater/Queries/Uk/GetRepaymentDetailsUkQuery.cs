using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetRepaymentDetails")]
	public partial class GetRepaymentDetailsUkQuery : ApiRequest<GetRepaymentDetailsUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
