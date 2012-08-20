using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Integration.Merchants.CsApi.Commands.Uk
{
	[XmlRoot("CsSaveMerchantContactDetails")]
	public partial class CsSaveMerchantContactDetailsUkCommand : CsRequest<CsSaveMerchantContactDetailsUkCommand>
	{
		public Object MerchantId { get; set; }
		public Object ContactName { get; set; }
		public Object Email { get; set; }
		public Object Phone { get; set; }
		public Object Title { get; set; }
	}
}
