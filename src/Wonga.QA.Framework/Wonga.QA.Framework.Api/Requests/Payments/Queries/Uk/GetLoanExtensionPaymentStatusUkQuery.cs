using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
	[XmlRoot("GetLoanExtensionPaymentStatus")]
	public partial class GetLoanExtensionPaymentStatusUkQuery : ApiRequest<GetLoanExtensionPaymentStatusUkQuery>
	{
		public Object ExtensionId { get; set; }
	}
}
