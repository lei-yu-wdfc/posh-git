using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
	[XmlRoot("ChangePasswordByTokenAndEmail")]
	public partial class ChangePasswordByTokenAndEmailCommand : ApiRequest<ChangePasswordByTokenAndEmailCommand>
	{
		public Object Token { get; set; }
		public Object Email { get; set; }
		public Object NewPassword { get; set; }
	}
}
