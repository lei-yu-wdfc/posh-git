using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
	[XmlRoot("ResetPassword")]
	public partial class ResetPasswordCommand : ApiRequest<ResetPasswordCommand>
	{
		public Object PwdResetKey { get; set; }
		public Object NewPassword { get; set; }
	}
}
