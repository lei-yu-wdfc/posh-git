using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.Csapi.Commands
{
	[XmlRoot("CsCreateDmpRepaymentArrangement")]
	public partial class CsCreateDmpRepaymentArrangementCommand : CsRequest<CsCreateDmpRepaymentArrangementCommand>
	{
		public Object ApplicationId { get; set; }
		public Object AccountId { get; set; }
		public Object RepaymentAmount { get; set; }
		public Object ArrangementDetails { get; set; }
	}
}
