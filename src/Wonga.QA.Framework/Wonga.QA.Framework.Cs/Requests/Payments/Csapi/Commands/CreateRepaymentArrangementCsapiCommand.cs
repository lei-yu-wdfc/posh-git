using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.Csapi.Commands
{
	[XmlRoot("CreateRepaymentArrangementCsapi")]
	public partial class CreateRepaymentArrangementCsapiCommand : CsRequest<CreateRepaymentArrangementCsapiCommand>
	{
		public Object ApplicationId { get; set; }
		public Object AccountId { get; set; }
		public Object EffectiveBalance { get; set; }
		public Object RepaymentAmount { get; set; }
		public Object ArrangementDetails { get; set; }
	}
}
