using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("CompleteMobilePhoneVerification")]
	public partial class CompleteMobilePhoneVerificationCommand : ApiRequest<CompleteMobilePhoneVerificationCommand>
	{
		public Object VerificationId { get; set; }
		public Object Pin { get; set; }
	}
}
