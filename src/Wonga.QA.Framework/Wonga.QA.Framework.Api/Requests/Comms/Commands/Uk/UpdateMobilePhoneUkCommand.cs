using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
	[XmlRoot("UpdateMobilePhone")]
	public partial class UpdateMobilePhoneUkCommand : ApiRequest<UpdateMobilePhoneUkCommand>
	{
		public Object AccountId { get; set; }
		public Object MobilePhone { get; set; }
	}
}
