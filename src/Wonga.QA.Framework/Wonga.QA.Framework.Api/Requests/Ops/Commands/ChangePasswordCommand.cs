using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
	[XmlRoot("ChangePassword")]
	public partial class ChangePasswordCommand : ApiRequest<ChangePasswordCommand>
	{
		public Object AccountId { get; set; }
		public Object CurrentPassword { get; set; }
		public Object NewPassword { get; set; }
	}
}
