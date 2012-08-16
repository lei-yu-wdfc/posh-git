using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
	[XmlRoot("VerifyPaylaterApplication")]
	public partial class VerifyPaylaterApplicationUkCommand : ApiRequest<VerifyPaylaterApplicationUkCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
	}
}
