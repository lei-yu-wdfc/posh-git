using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("SendPasswordResetEmail")]
	public partial class SendPasswordResetEmailCommand : ApiRequest<SendPasswordResetEmailCommand>
	{
		public Object NotificationId { get; set; }
		public Object Email { get; set; }
		public Object UriMask { get; set; }
	}
}
