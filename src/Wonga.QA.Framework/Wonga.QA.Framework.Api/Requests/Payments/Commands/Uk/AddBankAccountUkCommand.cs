using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk
{
	[XmlRoot("AddBankAccount")]
	public partial class AddBankAccountUkCommand : ApiRequest<AddBankAccountUkCommand>
	{
		public Object AccountId { get; set; }
		public Object BankAccountId { get; set; }
		public Object BankName { get; set; }
		public Object BankCode { get; set; }
		public Object AccountNumber { get; set; }
		public Object HolderName { get; set; }
		public Object AccountOpenDate { get; set; }
		public Object CountryCode { get; set; }
		public Object IsPrimary { get; set; }
		public Object IsValid { get; set; }
	}
}
