using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk
{
	[XmlRoot("RiskAddHomePhone")]
	public partial class RiskAddHomePhoneUkCommand : ApiRequest<RiskAddHomePhoneUkCommand>
	{
		public Object AccountId { get; set; }
		public Object HomePhone { get; set; }
	}
}
