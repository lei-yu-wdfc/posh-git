using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
	[XmlRoot("SaveCustomerFeedbackStory")]
	public partial class SaveCustomerFeedbackStoryCommand : ApiRequest<SaveCustomerFeedbackStoryCommand>
	{
		public Object AccountId { get; set; }
		public Object Story { get; set; }
		public Object CustomerName { get; set; }
		public Object AllowContact { get; set; }
	}
}
