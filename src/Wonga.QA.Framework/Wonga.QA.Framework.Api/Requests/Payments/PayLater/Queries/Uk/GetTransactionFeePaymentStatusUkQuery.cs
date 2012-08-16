using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetTransactionFeePaymentStatus")]
	public partial class GetTransactionFeePaymentStatusUkQuery : ApiRequest<GetTransactionFeePaymentStatusUkQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
