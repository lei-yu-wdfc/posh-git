using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	[XmlRoot("SubmitUidAnswers")]
	public partial class SubmitUidAnswersCommand : ApiRequest<SubmitUidAnswersCommand>
	{
		public Object UserActionId { get; set; }
		public Object Answers { get; set; }
	}
}
