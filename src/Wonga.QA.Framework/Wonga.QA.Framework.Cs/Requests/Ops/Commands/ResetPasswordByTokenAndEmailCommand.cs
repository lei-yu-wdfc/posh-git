using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands
{
	[XmlRoot("ResetPasswordByTokenAndEmail")]
	public partial class ResetPasswordByTokenAndEmailCommand : CsRequest<ResetPasswordByTokenAndEmailCommand>
	{
		public Object Email { get; set; }
	}
}
