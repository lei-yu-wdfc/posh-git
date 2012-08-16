using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	[XmlRoot("CancelUserAction")]
	public partial class CancelUserActionCommand : ApiRequest<CancelUserActionCommand>
	{
		public Object UserActionId { get; set; }
	}
}
