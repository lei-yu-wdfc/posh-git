using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
	[XmlRoot("GeneratePasswordResetKey")]
	public partial class GeneratePasswordResetKeyCommand : ApiRequest<GeneratePasswordResetKeyCommand>
	{
		public Object NotificationId { get; set; }
		public Object Complexity { get; set; }
		public Object Login { get; set; }
	}
}
