using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetPayLaterCustomerTransactionsStatus")]
	public partial class GetPayLaterCustomerTransactionsStatusUkQuery : ApiRequest<GetPayLaterCustomerTransactionsStatusUkQuery>
	{
		public Object AccountId { get; set; }
	}
}
