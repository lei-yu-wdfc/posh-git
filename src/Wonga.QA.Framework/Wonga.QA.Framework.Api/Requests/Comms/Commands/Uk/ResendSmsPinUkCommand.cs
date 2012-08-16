using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
	[XmlRoot("ResendSmsPin")]
	public partial class ResendSmsPinUkCommand : ApiRequest<ResendSmsPinUkCommand>
	{
		public Object AccountId { get; set; }
		public Object MobilePhone { get; set; }
	}
}
