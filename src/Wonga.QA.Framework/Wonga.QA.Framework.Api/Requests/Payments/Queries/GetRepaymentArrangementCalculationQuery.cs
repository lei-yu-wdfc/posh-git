using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetRepaymentArrangementCalculation")]
	public partial class GetRepaymentArrangementCalculationQuery : ApiRequest<GetRepaymentArrangementCalculationQuery>
	{
		public Object AccountId { get; set; }
		public Object NumberOfMonths { get; set; }
		public Object RepaymentFrequency { get; set; }
		public Object FirstRepaymentDate { get; set; }
	}
}
