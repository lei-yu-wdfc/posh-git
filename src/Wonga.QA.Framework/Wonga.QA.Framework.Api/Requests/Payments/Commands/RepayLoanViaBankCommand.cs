using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("RepayLoanViaBank")]
	public partial class RepayLoanViaBankCommand : ApiRequest<RepayLoanViaBankCommand>
	{
		public Object ApplicationId { get; set; }
		public Object CashEntityId { get; set; }
		public Object Amount { get; set; }
		public Object RepaymentRequestId { get; set; }
		public Object ActionDate { get; set; }
	}
}
