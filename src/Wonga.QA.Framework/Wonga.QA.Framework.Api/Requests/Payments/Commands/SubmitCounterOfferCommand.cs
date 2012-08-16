using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SubmitCounterOffer")]
	public partial class SubmitCounterOfferCommand : ApiRequest<SubmitCounterOfferCommand>
	{
		public Object ApplicationId { get; set; }
		public Object UserActionId { get; set; }
		public Object NewLoanAmount { get; set; }
	}
}
