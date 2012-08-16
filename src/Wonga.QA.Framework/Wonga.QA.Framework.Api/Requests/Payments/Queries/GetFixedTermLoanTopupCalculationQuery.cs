using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetFixedTermLoanTopupCalculation")]
	public partial class GetFixedTermLoanTopupCalculationQuery : ApiRequest<GetFixedTermLoanTopupCalculationQuery>
	{
		public Object ApplicationId { get; set; }
		public Object TopupAmount { get; set; }
	}
}
