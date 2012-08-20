using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands
{
	[XmlRoot("GeneratePasswordResetKey")]
	public partial class GeneratePasswordResetKeyCommand : CsRequest<GeneratePasswordResetKeyCommand>
	{
		public Object NotificationId { get; set; }
		public Object Complexity { get; set; }
		public Object Login { get; set; }
	}
}
