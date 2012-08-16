using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("RepayLoanViaCard")]
	public partial class RepayLoanViaCardCommand : ApiRequest<RepayLoanViaCardCommand>
	{
		public Object PaymentRequestId { get; set; }
		public Object ApplicationId { get; set; }
		public Object PaymentCardId { get; set; }
		public Object Amount { get; set; }
		public Object PaymentCardCv2 { get; set; }
	}
}
