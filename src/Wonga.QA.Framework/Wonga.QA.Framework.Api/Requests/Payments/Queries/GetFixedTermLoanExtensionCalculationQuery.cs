using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetFixedTermLoanExtensionCalculation")]
	public partial class GetFixedTermLoanExtensionCalculationQuery : ApiRequest<GetFixedTermLoanExtensionCalculationQuery>
	{
		public Object ApplicationId { get; set; }
		public Object ExtendDate { get; set; }
	}
}
