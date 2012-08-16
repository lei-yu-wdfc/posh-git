using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetRepayLoanStatus")]
	public partial class GetRepayLoanStatusQuery : ApiRequest<GetRepayLoanStatusQuery>
	{
		public Object AccountId { get; set; }
	}
}
