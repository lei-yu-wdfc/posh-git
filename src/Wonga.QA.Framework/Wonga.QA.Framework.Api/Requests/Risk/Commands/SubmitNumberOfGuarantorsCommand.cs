using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	[XmlRoot("SubmitNumberOfGuarantors")]
	public partial class SubmitNumberOfGuarantorsCommand : ApiRequest<SubmitNumberOfGuarantorsCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object NumberOfGuarantors { get; set; }
	}
}
