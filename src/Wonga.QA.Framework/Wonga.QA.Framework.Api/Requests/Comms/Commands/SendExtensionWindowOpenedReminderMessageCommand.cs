using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("SendExtensionWindowOpenedReminderMessage")]
	public partial class SendExtensionWindowOpenedReminderMessageCommand : ApiRequest<SendExtensionWindowOpenedReminderMessageCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object NotificationDate { get; set; }
		public Object CustomerTimeZoneName { get; set; }
		public Object SendEmailNotification { get; set; }
		public Object SendSmsNotification { get; set; }
	}
}
