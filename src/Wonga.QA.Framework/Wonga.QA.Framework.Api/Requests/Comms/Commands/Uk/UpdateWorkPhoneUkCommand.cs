using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
	[XmlRoot("UpdateWorkPhone")]
	public partial class UpdateWorkPhoneUkCommand : ApiRequest<UpdateWorkPhoneUkCommand>
	{
		public Object AccountId { get; set; }
		public Object WorkPhone { get; set; }
	}
}
