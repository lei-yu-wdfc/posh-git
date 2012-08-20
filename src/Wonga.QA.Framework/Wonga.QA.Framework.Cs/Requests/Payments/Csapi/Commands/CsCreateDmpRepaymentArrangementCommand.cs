using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
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