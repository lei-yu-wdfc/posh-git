using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SignApplication")]
	public partial class SignApplicationCommand : ApiRequest<SignApplicationCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
	}
}
