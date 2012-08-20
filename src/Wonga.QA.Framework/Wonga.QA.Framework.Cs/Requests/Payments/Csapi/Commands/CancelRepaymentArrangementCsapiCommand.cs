using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.Csapi.Commands
{
	[XmlRoot("CancelRepaymentArrangementCsapi")]
	public partial class CancelRepaymentArrangementCsapiCommand : CsRequest<CancelRepaymentArrangementCsapiCommand>
	{
		public Object RepaymentArrangementId { get; set; }
	}
}
