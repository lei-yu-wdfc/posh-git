using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetRepaymentArrangementParameters")]
	public partial class GetRepaymentArrangementParametersQuery : ApiRequest<GetRepaymentArrangementParametersQuery>
	{
		public Object AccountId { get; set; }
	}
}
