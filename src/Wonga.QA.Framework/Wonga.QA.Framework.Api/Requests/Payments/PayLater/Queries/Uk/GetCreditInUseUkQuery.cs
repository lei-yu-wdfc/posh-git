using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
	[XmlRoot("GetCreditInUse")]
	public partial class GetCreditInUseUkQuery : ApiRequest<GetCreditInUseUkQuery>
	{
		public Object AccountId { get; set; }
	}
}
