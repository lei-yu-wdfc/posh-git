using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands
{
	[XmlRoot("ChangePassword")]
	public partial class ChangePasswordCommand : CsRequest<ChangePasswordCommand>
	{
		public Object AccountId { get; set; }
		public Object CurrentPassword { get; set; }
		public Object NewPassword { get; set; }
	}
}
