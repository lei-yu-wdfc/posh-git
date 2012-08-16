using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("CreateFixedTermLoanTopup")]
	public partial class CreateFixedTermLoanTopupCommand : ApiRequest<CreateFixedTermLoanTopupCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object FixedTermLoanTopupId { get; set; }
		public Object TopupAmount { get; set; }
	}
}
