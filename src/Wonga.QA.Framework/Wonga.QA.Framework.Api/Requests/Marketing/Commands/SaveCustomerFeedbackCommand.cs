using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
	[XmlRoot("SaveCustomerFeedback")]
	public partial class SaveCustomerFeedbackCommand : ApiRequest<SaveCustomerFeedbackCommand>
	{
		public Object AccountId { get; set; }
		public Object Feedback { get; set; }
	}
}
