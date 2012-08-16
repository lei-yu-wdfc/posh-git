using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetNextRepaymentDate")]
	public partial class GetNextRepaymentDateUkQuery : ApiRequest<GetNextRepaymentDateUkQuery>
	{
		public Object AccountId { get; set; }
	}
}
