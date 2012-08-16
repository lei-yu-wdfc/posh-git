using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetBanks")]
	public partial class GetBanksQuery : ApiRequest<GetBanksQuery>
	{
	}
}
