using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
	[XmlRoot("SaveCustomerFeedbackRate")]
	public partial class SaveCustomerFeedbackRateCommand : ApiRequest<SaveCustomerFeedbackRateCommand>
	{
		public Object AccountId { get; set; }
		public Object Rate { get; set; }
	}
}
