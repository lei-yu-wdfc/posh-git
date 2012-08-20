using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.Csapi.Commands
{
	[XmlRoot("CancelArrearsRepresentmentsCsapi")]
	public partial class CancelArrearsRepresentmentsCsapiCommand : CsRequest<CancelArrearsRepresentmentsCsapiCommand>
	{
		public Object ApplicationId { get; set; }
		public Object ArrearsRepaymentId { get; set; }
	}
}
