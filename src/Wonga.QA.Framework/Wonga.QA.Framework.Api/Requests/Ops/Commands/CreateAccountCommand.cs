using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
	[XmlRoot("CreateAccount")]
	public partial class CreateAccountCommand : ApiRequest<CreateAccountCommand>
	{
		public Object AccountId { get; set; }
		public Object Login { get; set; }
		public Object Password { get; set; }
	}
}
