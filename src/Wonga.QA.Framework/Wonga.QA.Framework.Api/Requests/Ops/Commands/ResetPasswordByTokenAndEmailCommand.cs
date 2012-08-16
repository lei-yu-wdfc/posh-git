using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
	[XmlRoot("ResetPasswordByTokenAndEmail")]
	public partial class ResetPasswordByTokenAndEmailCommand : ApiRequest<ResetPasswordByTokenAndEmailCommand>
	{
		public Object Email { get; set; }
	}
}
