using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
	[XmlRoot("RegisterPrePaidCardEmailCommand")]
	public partial class RegisterPrePaidCardEmailCommandCommand : ApiRequest<RegisterPrePaidCardEmailCommandCommand>
	{
		public Object Email { get; set; }
	}
}
