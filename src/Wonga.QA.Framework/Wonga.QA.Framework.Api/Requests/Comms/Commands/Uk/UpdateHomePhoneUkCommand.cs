using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
	[XmlRoot("UpdateHomePhone")]
	public partial class UpdateHomePhoneUkCommand : ApiRequest<UpdateHomePhoneUkCommand>
	{
		public Object AccountId { get; set; }
		public Object HomePhone { get; set; }
	}
}
