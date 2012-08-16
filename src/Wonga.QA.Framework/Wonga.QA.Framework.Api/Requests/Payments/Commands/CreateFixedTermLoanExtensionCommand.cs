using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("CreateFixedTermLoanExtension")]
	public partial class CreateFixedTermLoanExtensionCommand : ApiRequest<CreateFixedTermLoanExtensionCommand>
	{
		public Object ApplicationId { get; set; }
		public Object ExtensionId { get; set; }
		public Object ExtendDate { get; set; }
		public Object PaymentCardId { get; set; }
		public Object PaymentCardCv2 { get; set; }
		public Object PartPaymentAmount { get; set; }
	}
}
