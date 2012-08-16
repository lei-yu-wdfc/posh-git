using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("SendVerificationEmail")]
	public partial class SendVerificationEmailCommand : ApiRequest<SendVerificationEmailCommand>
	{
		public Object AccountId { get; set; }
		public Object Email { get; set; }
		public Object UriFragment { get; set; }
	}
}
