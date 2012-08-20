using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.Merchants.CsApi.Queries.Uk
{
	[XmlRoot("CsGetMerchantBankDetails")]
	public partial class CsGetMerchantBankDetailsUkQuery : CsRequest<CsGetMerchantBankDetailsUkQuery>
	{
		public Object MerchantId { get; set; }
	}
}
