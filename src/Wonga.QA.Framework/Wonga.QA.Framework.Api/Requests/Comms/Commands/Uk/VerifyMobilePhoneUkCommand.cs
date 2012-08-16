using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
	[XmlRoot("VerifyMobilePhone")]
	public partial class VerifyMobilePhoneUkCommand : ApiRequest<VerifyMobilePhoneUkCommand>
	{
		public Object VerificationId { get; set; }
		public Object AccountId { get; set; }
		public Object MobilePhone { get; set; }
		public Object Forename { get; set; }
	}
}
