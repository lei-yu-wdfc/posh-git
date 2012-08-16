using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
	[XmlRoot("GetRepayLoanQuote")]
	public partial class GetRepayLoanQuoteUkQuery : ApiRequest<GetRepayLoanQuoteUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
