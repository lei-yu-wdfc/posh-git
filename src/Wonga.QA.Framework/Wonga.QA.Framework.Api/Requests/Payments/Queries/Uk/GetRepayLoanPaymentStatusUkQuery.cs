using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
	[XmlRoot("GetRepayLoanPaymentStatus")]
	public partial class GetRepayLoanPaymentStatusUkQuery : ApiRequest<GetRepayLoanPaymentStatusUkQuery>
	{
		public Object ApplicationId { get; set; }
		public Object RepaymentRequestId { get; set; }
	}
}
