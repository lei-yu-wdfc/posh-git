using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands.Uk
{
	[XmlRoot("SavePasswordRecoveryDetails")]
	public partial class SavePasswordRecoveryDetailsUkCommand : ApiRequest<SavePasswordRecoveryDetailsUkCommand>
	{
		public Object AccountId { get; set; }
		public Object SecretQuestion { get; set; }
		public Object SecretAnswer { get; set; }
	}
}
