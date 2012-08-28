using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Uk
{
	[XmlRoot("CreateFixedTermLoanApplication")]
	public partial class CreateFixedTermLoanApplicationUkCommand : ApiRequest<CreateFixedTermLoanApplicationUkCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object PaymentCardId { get; set; }
		public Object BankAccountId { get; set; }
		public Object Currency { get; set; }
		public Object PromiseDate { get; set; }
		public Object LoanAmount { get; set; }
		public Object PromoCodeId { get; set; }
		public Object AffiliateId { get; set; }
	}
}
