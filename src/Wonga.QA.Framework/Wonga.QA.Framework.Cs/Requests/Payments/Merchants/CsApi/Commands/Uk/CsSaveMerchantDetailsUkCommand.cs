using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.Merchants.CsApi.Commands.Uk
{
	[XmlRoot("CsSaveMerchantDetails")]
	public partial class CsSaveMerchantDetailsUkCommand : CsRequest<CsSaveMerchantDetailsUkCommand>
	{
		public Object MerchantId { get; set; }
		public Object FeeRate { get; set; }
		public Object FundedFeeRate { get; set; }
		public Object PaymentTerms { get; set; }
	}
}
