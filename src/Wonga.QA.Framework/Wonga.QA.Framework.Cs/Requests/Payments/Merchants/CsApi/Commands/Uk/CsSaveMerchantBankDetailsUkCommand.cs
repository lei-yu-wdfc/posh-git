using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Payments.Merchants.CsApi.Commands.Uk
{
	[XmlRoot("CsSaveMerchantBankDetails")]
	public partial class CsSaveMerchantBankDetailsUkCommand : CsRequest<CsSaveMerchantBankDetailsUkCommand>
	{
		public Object MerchantId { get; set; }
		public Object AccountName { get; set; }
		public Object AccountNumber { get; set; }
		public Object BankName { get; set; }
		public Object SortCode { get; set; }
	}
}
