using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetRepayLoanCalculation")]
	public partial class GetRepayLoanCalculationQuery : ApiRequest<GetRepayLoanCalculationQuery>
	{
		public Object ApplicationId { get; set; }
		public Object RepayAmount { get; set; }
		public Object RepayDate { get; set; }
	}
}
