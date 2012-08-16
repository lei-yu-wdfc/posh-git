using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands
{
	[XmlRoot("CompleteEmailVerification")]
	public partial class CompleteEmailVerificationCommand : ApiRequest<CompleteEmailVerificationCommand>
	{
		public Object AccountId { get; set; }
		public Object ChangeId { get; set; }
	}
}
