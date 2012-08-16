using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	[XmlRoot("SubmitApplicationBehaviour")]
	public partial class SubmitApplicationBehaviourCommand : ApiRequest<SubmitApplicationBehaviourCommand>
	{
		public Object ApplicationId { get; set; }
		public Object TermSliderPosition { get; set; }
		public Object AmountSliderPosition { get; set; }
	}
}
