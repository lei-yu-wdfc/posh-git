using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
	[XmlRoot("GetFixedTermLoanExtensionQuote")]
	public partial class GetFixedTermLoanExtensionQuoteUkQuery : ApiRequest<GetFixedTermLoanExtensionQuoteUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
