using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.BankValidate.Queries
{
	[XmlRoot("GetBankValidationResult")]
	public partial class GetBankValidationResultQuery : ApiRequest<GetBankValidationResultQuery>
	{
		public Object BankAccountId { get; set; }
		public Object BankName { get; set; }
		public Object BankCode { get; set; }
		public Object AccountNumber { get; set; }
		public Object HolderName { get; set; }
		public Object AccountOpenDate { get; set; }
		public Object CountryCode { get; set; }
	}
}
