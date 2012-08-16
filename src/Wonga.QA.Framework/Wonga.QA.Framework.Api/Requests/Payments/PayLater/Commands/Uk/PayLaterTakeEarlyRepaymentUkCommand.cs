using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk
{
	[XmlRoot("PayLaterTakeEarlyRepayment")]
	public partial class PayLaterTakeEarlyRepaymentUkCommand : ApiRequest<PayLaterTakeEarlyRepaymentUkCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object PaymentRequestId { get; set; }
		public Object PaymentCardId { get; set; }
		public Object Cv2 { get; set; }
		public Object Amount { get; set; }
	}
}
