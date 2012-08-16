using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetFixedTermLoanExtensionParameters")]
	public partial class GetFixedTermLoanExtensionParametersQuery : ApiRequest<GetFixedTermLoanExtensionParametersQuery>
	{
		public Object AccountId { get; set; }
	}
}
