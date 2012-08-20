using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands
{
	[XmlRoot("ChangePasswordByTokenAndEmail")]
	public partial class ChangePasswordByTokenAndEmailCommand : CsRequest<ChangePasswordByTokenAndEmailCommand>
	{
		public Object Token { get; set; }
		public Object Email { get; set; }
		public Object NewPassword { get; set; }
	}
}
