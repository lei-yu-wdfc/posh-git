using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
	[XmlRoot("VerifyFixedTermLoan")]
	public partial class VerifyFixedTermLoanCommand : ApiRequest<VerifyFixedTermLoanCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
	}
}
