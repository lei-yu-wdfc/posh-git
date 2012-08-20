using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.PayLater.Csapi.Commands.Uk
{
	[XmlRoot("ExecuteMerchantRefund")]
	public partial class ExecuteMerchantRefundUkCommand : CsRequest<ExecuteMerchantRefundUkCommand>
	{
		public Object ApplicationId { get; set; }
		public Object RefundAmount { get; set; }
	}
}
