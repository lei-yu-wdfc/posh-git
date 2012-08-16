using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetFixedTermLoanApplication")]
	public partial class GetFixedTermLoanApplicationQuery : ApiRequest<GetFixedTermLoanApplicationQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
