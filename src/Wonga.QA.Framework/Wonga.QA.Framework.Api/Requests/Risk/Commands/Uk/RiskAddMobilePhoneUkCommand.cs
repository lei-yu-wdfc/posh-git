using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk
{
	[XmlRoot("RiskAddMobilePhone")]
	public partial class RiskAddMobilePhoneUkCommand : ApiRequest<RiskAddMobilePhoneUkCommand>
	{
		public Object AccountId { get; set; }
		public Object MobilePhone { get; set; }
	}
}
