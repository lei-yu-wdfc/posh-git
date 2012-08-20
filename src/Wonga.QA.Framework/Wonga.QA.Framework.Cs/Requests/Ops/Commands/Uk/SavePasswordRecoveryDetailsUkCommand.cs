using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands.Uk
{
	[XmlRoot("SavePasswordRecoveryDetails")]
	public partial class SavePasswordRecoveryDetailsUkCommand : CsRequest<SavePasswordRecoveryDetailsUkCommand>
	{
		public Object AccountId { get; set; }
		public Object SecretQuestion { get; set; }
		public Object SecretAnswer { get; set; }
	}
}
