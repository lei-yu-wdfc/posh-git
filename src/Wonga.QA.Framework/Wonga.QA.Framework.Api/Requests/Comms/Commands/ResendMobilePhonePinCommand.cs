using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("ResendMobilePhonePin")]
	public partial class ResendMobilePhonePinCommand : ApiRequest<ResendMobilePhonePinCommand>
	{
		public Object VerificationId { get; set; }
		public Object Forename { get; set; }
	}
}
