using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands
{
	[XmlRoot("CreateAccount")]
	public partial class CreateAccountCommand : CsRequest<CreateAccountCommand>
	{
		public Object AccountId { get; set; }
		public Object Login { get; set; }
		public Object Password { get; set; }
	}
}
