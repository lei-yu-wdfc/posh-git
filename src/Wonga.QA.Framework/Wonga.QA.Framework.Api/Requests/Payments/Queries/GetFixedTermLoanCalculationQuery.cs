using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetFixedTermLoanCalculation")]
	public partial class GetFixedTermLoanCalculationQuery : ApiRequest<GetFixedTermLoanCalculationQuery>
	{
		public Object LoanAmount { get; set; }
		public Object Term { get; set; }
		public Object PromoCodeId { get; set; }
        public Object TransmissionFeeDiscount { get; set; }
	}
}
