using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("CreateScheduledPaymentRequest")]
	public partial class CreateScheduledPaymentRequestCommand : ApiRequest<CreateScheduledPaymentRequestCommand>
	{
		public Object ApplicationId { get; set; }
		public Object ExtensionId { get; set; }
	}
}
