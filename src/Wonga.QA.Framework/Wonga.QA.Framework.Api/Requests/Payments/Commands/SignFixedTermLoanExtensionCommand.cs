using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SignFixedTermLoanExtension")]
	public partial class SignFixedTermLoanExtensionCommand : ApiRequest<SignFixedTermLoanExtensionCommand>
	{
		public Object ApplicationId { get; set; }
		public Object ExtensionId { get; set; }
	}
}
