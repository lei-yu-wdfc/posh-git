using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SetFundsTransferMethodCommand")]
	public partial class SetFundsTransferMethodCommandCommand : ApiRequest<SetFundsTransferMethodCommandCommand>
	{
		public Object ApplicationId { get; set; }
		public Object TransferMethod { get; set; }
	}
}
