using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetRepayLoanParameters")]
	public partial class GetRepayLoanParametersQuery : ApiRequest<GetRepayLoanParametersQuery>
	{
		public Object AccountId { get; set; }
	}
}
