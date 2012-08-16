using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	[XmlRoot("RiskCreateFixedTermLoanApplication")]
	public partial class RiskCreateFixedTermLoanApplicationCommand : ApiRequest<RiskCreateFixedTermLoanApplicationCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object PaymentCardId { get; set; }
		public Object BankAccountId { get; set; }
		public Object Currency { get; set; }
		public Object PromiseDate { get; set; }
		public Object LoanAmount { get; set; }
	}
}
