using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("DeletePaymentCard")]
	public partial class DeletePaymentCardCommand : ApiRequest<DeletePaymentCardCommand>
	{
		public Object AccountId { get; set; }
		public Object PaymentCardId { get; set; }
	}
}
