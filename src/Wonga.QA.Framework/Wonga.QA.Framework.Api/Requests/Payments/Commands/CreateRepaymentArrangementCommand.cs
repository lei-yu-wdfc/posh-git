using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("CreateRepaymentArrangement")]
	public partial class CreateRepaymentArrangementCommand : ApiRequest<CreateRepaymentArrangementCommand>
	{
		public Object ApplicationId { get; set; }
		public Object Frequency { get; set; }
		public Object RepaymentDates { get; set; }
		public Object NumberOfMonths { get; set; }
	}
}
