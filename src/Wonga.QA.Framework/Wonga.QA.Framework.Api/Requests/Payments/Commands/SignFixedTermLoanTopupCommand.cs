using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SignFixedTermLoanTopup")]
	public partial class SignFixedTermLoanTopupCommand : ApiRequest<SignFixedTermLoanTopupCommand>
	{
		public Object AccountId { get; set; }
		public Object FixedTermLoanTopupId { get; set; }
	}
}
