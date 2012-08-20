using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands
{
	[XmlRoot("ResetPassword")]
	public partial class ResetPasswordCommand : CsRequest<ResetPasswordCommand>
	{
		public Object PwdResetKey { get; set; }
		public Object NewPassword { get; set; }
	}
}
